using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Source.Gameplay.Characters.Enemy;
using Source.Gameplay.Environment;
using Source.Gameplay.Extensions;
using Source.Services;
using Source.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class StickmanController : MonoBehaviour, 
        IStickmanController, IStickmanInfo, IInteractTarget, IAttackerGroup, IFormationGroup
    {
        private enum State {NONE, NORMAL, ATTACK, JUMP, FINISH}
        Transform IStickmanController.Transform => _tr;
        private int StickmanCount => _characters.Count;
        int IFormationGroup.Count => _characters.Count;
        bool IAttackerGroup.IsAlive => StickmanCount > 0;
        private bool IsCanAttack => _enemyGroup != null && _enemyGroup.IsAlive;
        Vector3 Center => (_container.childCount > 0) ? 
            _container.GetChild(0).position : _tr.position;

        [SerializeField] private StickmenViewInfo _counter;
        private TowerProvider _towerProvider;
        private Transform _container;
        private StickmanControllerData _config;
        private StickmanFactory _factory;
        private Transform _tr;
        private List<Stickman> _characters = new();
        private IEnemyGroup _enemyGroup;
        private State _state;

        public event Action<int, int> ChangeStickmanCountEvent;
        public event Action FailureEvent;
        public event Action<int> CompletedEvent;
        public event Action<Transform> FinishStateEvent;


        public void Init(StickmanControllerData config, StickmanFactory factory)
        {
            _config = config;
            _factory = factory;
            _tr = transform;
            _counter.Init(this);
            _towerProvider = new TowerProvider(_config.Tower);

            _container = new GameObject("Container").transform;
            _container.SetLocalPositionAndRotation(_tr.position, _tr.rotation);
            _container.SetParent(_tr);

            PopulateAsync(_config.StartStickmanCount);

            SetState(State.NORMAL);
        }
        

        private void SetState(State s) 
        {
            _state = s;
        }


        private async void PopulateAsync(int num)
        {
            var prev = StickmanCount;

            for (int i = 0; i < num; i++)
            {
                var stickman = _factory.Get(StickmanType.SIMPLE_STICKMAN);
                stickman.transform.SetPositionAndRotation(_container.position, _container.rotation);
                stickman.transform.SetParent(_container);

                stickman.DieEvent += OnStickmanDie;
                stickman.JumpEvent += OnStickmanJump;
                stickman.FinishEvent += OnStickmanFinish;

                _characters.Add(stickman);

                if (i % 20 == 0) await Task.Yield();
            }

            Formation(2f);

            ChangeStickmanCountEvent?.Invoke(prev, StickmanCount);
        }
       

        private void UnSubscribe(Stickman stickman)
        {
            stickman.DieEvent -= OnStickmanDie;
            stickman.JumpEvent -= OnStickmanJump;
            stickman.FinishEvent -= OnStickmanFinish;
        }    


        void IFormationGroup.Refresh() => Formation(isFirstInclude: true);   


        void IFormationGroup.FinishFormation()
        {
            var positions = _towerProvider.Create(StickmanCount);
            var index = 0;

            foreach(var pos in positions)
            {
                _characters[index].SetLocalPositionAndRotation
                (
                    pos, 
                    _container.transform.forward,
                    1f
                );

                index++;
            }

            _counter.Hide();
            SetState(State.FINISH);
            FinishStateEvent?.Invoke(_characters.Last().transform);
        }  


        private void Formation(float duration = 1f, bool isFirstInclude = false)
        {
            var start = (isFirstInclude) ? 0 : 1;
            
            for (int i = start; i < StickmanCount; i++)
            {    
                var newPos = UnitExtensions.Formation
                    .GetPositionInSpiralFormation(_config.Formation.UnitDistanceFactor, _config.Formation.UnitRadius, i);

                _characters[i].SetLocalPositionAndRotation
                (
                    newPos, 
                    _container.transform.forward,
                    duration
                );                
            }
        }
       

        void IStickmanController.OnUpdate()
        {     
            switch(_state)
            {
                case State.ATTACK:

                    if (IsCanAttack)
                    {
                        Attack(_enemyGroup.Center);
                        _enemyGroup.Attack(Center);
                    }

                break;

                case State.JUMP:

                    _tr.Translate(_tr.forward * _config.Movement.VerticalSpeed * Time.deltaTime);

                break;   

                case State.FINISH:

                    var speed = _config.Movement.VerticalSpeed * Time.deltaTime;
                    
                    _tr.Translate(_tr.forward * speed);
                    
                    var pos = _tr.position;
                    pos.x = Mathf.Lerp(pos.x, 0f, speed);
                    _tr.position = pos;

                break;             
            }
        }


        private void Attack(Vector3 attackPosition)
        {
            _tr.Translate(_tr.forward * _config.Movement.AttackMoveSpeed * Time.deltaTime);
            
            for (int i = 0; i < StickmanCount; i++)
            {
                var cur = _characters[i];                
                cur.MoveToPosition(attackPosition, _config.Unit.AttackSpeed, _config.Unit.AttackRotateSpeed);  
                cur.PlayRun();
            } 
        }
        

        void IStickmanController.Move(float x, float z)
        {
            if (_state != State.NORMAL) return;

            _tr.position = GetPosition(x, z);
            Run();     
        }

        private Vector3 GetPosition(float x, float z)
        {
            var curPos = _tr.position;
            var xPos = Mathf.Lerp(curPos.x, x, Time.deltaTime * _config.Movement.HorizontalSpeed);
            var zPos = Mathf.Lerp(curPos.z, z, Time.deltaTime * _config.Movement.VerticalSpeed);
            
            var newPos = new Vector3(xPos, curPos.y, zPos);
            ClampHorizontal(ref newPos);

            return newPos;        
        }


        private void ClampHorizontal(ref Vector3 newPos)
        {
            var countProgress = (1f - Mathf.InverseLerp(_config.MinCount, _config.MaxCount, StickmanCount)); 
            var limit = _config.Movement.HorizontalMovementLimit * countProgress;  
            newPos.x = Mathf.Clamp(newPos.x, -limit, limit);
        }


        private void Run()
        {
            _characters.ForEach(s => s.PlayRun());
        }


        void IStickmanController.Stop()
        {
            if (_state != State.NORMAL) return;
            
            _characters.ForEach(s => s.PlayStop()); 
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IInteractable item)) { item.Interact(this); } 

            if (other.TryGetComponent(out IEnemyGroup enemyGroup)) 
            { 
                enemyGroup.KillAllUnitsEvent += OnAttackCompleted;
                enemyGroup.PrepareForAttack(this);
                _enemyGroup = enemyGroup;

                SetState(State.ATTACK);
            } 
        }


        private void OnAttackCompleted(IEnemyGroup group)
        {
            group.KillAllUnitsEvent -= OnAttackCompleted;
            SetState(State.NORMAL);
            Formation(_config.Unit.ReturnToOriginDuration, true);
        }


        void IInteractTarget.MultiplyStickman(int multiplier)
        {
            PopulateAsync(StickmanCount * multiplier);
        }


        void IInteractTarget.AddStickman(int amount)
        {
            PopulateAsync(amount);
        }
        

        private void OnStickmanJump(Stickman stickman, float jumpPower)
        {
            stickman.Jump(jumpPower, _config.Movement.JumpDuration);

            if (_state == State.JUMP) return;

            SetState(State.JUMP);
            StartCoroutine(ResetJumpState(_config.Movement.JumpDuration));

            IEnumerator ResetJumpState(float duration)
            {
                yield return new WaitForSeconds(duration);
                SetState(State.NORMAL);
                Formation(isFirstInclude: true);
            }
        }


        private void RemoveStickman(Stickman stickman, Action onZeroCount)
        {
            var prev = StickmanCount;
            
            UnSubscribe(stickman);

            _characters.Remove(stickman);

            ChangeStickmanCountEvent?.Invoke(prev, StickmanCount);

            if (StickmanCount > 0) return;

            onZeroCount?.Invoke();
        }


        private void OnStickmanDie(Stickman stickman)
        {
            RemoveStickman(stickman, Failure); 
        }


        private void OnStickmanFinish(Stickman stickman, int points)
        {            
            stickman.Release();
            RemoveStickman(stickman, () => Completed(points));
        }


        private void Completed(int points)
        {
            SetState(State.NONE);
            CompletedEvent?.Invoke(points);
        }


        private void Failure()
        {
            _enemyGroup?.StopAttack();
            FailureEvent?.Invoke();
            _counter.Hide();
        }
    }
}