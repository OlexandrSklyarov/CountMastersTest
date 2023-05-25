using System;
using System.Collections.Generic;
using DG.Tweening;
using Source.Data;
using Source.Gameplay.Characters.Enemy;
using Source.Gameplay.Environment;
using Source.Gameplay.Extensions;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class StickmanController : MonoBehaviour, 
        IStickmanController, IStickmanInfo, IInteractTarget, IAttackerGroup
    {
        private enum State {NORMAL, ATTACK}
        Transform IStickmanController.Transform => _tr;
        private int StickmanCount => _characters.Count;
        bool IAttackerGroup.IsAlive => StickmanCount > 0;
        private bool IsCanAttack => _state == State.ATTACK && _enemyGroup != null && _enemyGroup.IsAlive;
        Vector3 Center => (_container.childCount > 0) ? 
            _container.GetChild(0).position : _tr.position;

        [SerializeField] private StickmenViewInfo _counter;

        private Transform _container;
        private StickmanControllerData _config;
        private StickmanFactory _factory;
        private Transform _tr;
        private List<Stickman> _characters = new();
        private IEnemyGroup _enemyGroup;
        private State _state;

        public event Action<int, int> ChangeStickmanCountEvent;
        public event Action FailureEvent;


        public void Init(StickmanControllerData config, StickmanFactory factory)
        {
            _config = config;
            _factory = factory;
            _tr = transform;
            _counter.Init(this);

            _container = new GameObject("Container").transform;
            _container.SetLocalPositionAndRotation(_tr.position, _tr.rotation);
            _container.SetParent(_tr);

            Populate(_config.StartStickmanCount);

            SetState(State.NORMAL);
        }
        

        private void SetState(State s) => _state = s;


        private void Populate(int num)
        {
            var prev = StickmanCount;

            for (int i = 0; i < num; i++)
            {
                var stickman = _factory.Get(StickmanType.SIMPLE_STICKMAN);
                stickman.transform.SetPositionAndRotation(_container.position, _container.rotation);
                stickman.transform.SetParent(_container);
                stickman.DieEvent += OnStickmanDie;

                _characters.Add(stickman);
            }

            Formation();

            ChangeStickmanCountEvent?.Invoke(prev, StickmanCount);
        }


        private void Formation(float duration = 1f, bool includeFirstUnit = false)
        {
            var start = (includeFirstUnit) ? 0 : 1;

            for (int i = start; i < StickmanCount; i++)
            {    
                var newPos = UnitExtensions.GetPositionInSpiralFormation(
                    _config.Unit.DistanceFactor, _config.Unit.Radius, i); 

                _container.GetChild(i).transform
                    .DOLocalMove(newPos, duration)
                    .SetEase(Ease.OutBack);
            }
        }
       

        void IStickmanController.OnUpdate()
        {
            if (IsCanAttack)
            {
                Attack(_enemyGroup.Center);
                _enemyGroup.Attack(Center);
            }
        }


        private void Attack(Vector3 attackPosition)
        {
            _tr.Translate(_tr.forward * _config.AttackMoveSpeed * Time.deltaTime);
            
            for (int i = 0; i < StickmanCount; i++)
            {
                var cur = _characters[i];                
                cur.MoveToPosition(attackPosition, _config.Unit.AttackSpeed, _config.Unit.AttackRotateSpeed);  
                cur.PlayRun();
            } 
        }


        void IStickmanController.Move(Vector3 newPos)
        {
            switch (_state)
            {
                case State.NORMAL:

                    ClampHorizontal(ref newPos);
                    _tr.position = newPos;
                    Run();

                    break;
            }            
        }


        private void ClampHorizontal(ref Vector3 newPos)
        {
            var countProgress = (1f - Mathf.InverseLerp(_config.MinCount, _config.MaxCount, StickmanCount)); 
            var limit = _config.HorizontalMovementLimit * countProgress;  
            newPos.x = Mathf.Clamp(newPos.x, -limit, limit);
        }


        private void Run()
        {
            _characters.ForEach(s => s.PlayRun());
        }


        void IStickmanController.Stop()
        {
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
            Formation(includeFirstUnit: true);
        }


        void IInteractTarget.MultiplyStickman(int multiplier)
        {
            Populate(StickmanCount * multiplier);
        }


        void IInteractTarget.AddStickman(int count)
        {
            Populate(StickmanCount + count);
        }


        private void OnStickmanDie(Stickman stickman)
        {
            var prev = StickmanCount;
            
            stickman.DieEvent -= OnStickmanDie;
            _characters.Remove(stickman);

            ChangeStickmanCountEvent?.Invoke(prev, StickmanCount);           

            if (StickmanCount > 0) return;

            FailureEvent?.Invoke();
            _counter.Hide();
        }
    }
}