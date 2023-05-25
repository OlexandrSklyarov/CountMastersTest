using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Vector3 IAttackerGroup.Center => _tr.position;
        List<Stickman> IAttackerGroup.Units => _characters;
        private bool IsCanAttack => _state == State.ATTACK && _enemyGroup != null;

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


        private void Formation(float duration = 1f)
        {
            for (int i = 1; i < StickmanCount; i++)
            {    
                var newPos = UnitExtensions.GetPositionInSpiralFormation(_config.UnitDistanceFactor, _config.UnitRadius, i);

                _container.GetChild(i).transform
                    .DOLocalMove(newPos, duration)
                    .SetEase(Ease.OutBack);
            }
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


        void IStickmanController.OnUpdate()
        {
            if (IsCanAttack)
            {
                Attack(_enemyGroup.Center);
            }
        }


        private void Attack(Vector3 attackPosition)
        {
            for (int i = 0; i < StickmanCount; i++)
            {
                var cur = _characters[i];

                if (!cur.IsActive) continue;
                
                cur.MoveToPosition(attackPosition, _config.UnitAttackSpeed, _config.UnitAttackRotateSpeed);  
            }            
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IInteractable item)) { item.Interact(this); } 

            if (other.TryGetComponent(out IEnemyGroup enemyGroup)) 
            { 
                enemyGroup.DestroyEvent += OnDestroyEnemyGroup;
                enemyGroup.SendAttack(this);
                _enemyGroup = enemyGroup;

                SetState(State.ATTACK);
            } 
        }


        private void OnDestroyEnemyGroup(IEnemyGroup group)
        {
            group.DestroyEvent -= OnDestroyEnemyGroup;
            SetState(State.NORMAL);
        }


        void IInteractTarget.MultiplyStickman(int multiplier)
        {
            Populate(StickmanCount * multiplier);
        }


        void IInteractTarget.AddStickman(int count)
        {
            Populate(StickmanCount + count);
        }


        void IStickmanController.Move(Vector3 newPos)
        {
            ClampHorizontal(ref newPos);
            _tr.position = newPos;

            _characters.ForEach(s => 
            {
                if (s.IsActive) s.PlayRun();
            });
        }


        private void ClampHorizontal(ref Vector3 newPos)
        {
            var limit = _config.HorizontalMovementLimit * 
                (1f - Mathf.InverseLerp(_config.MinCount, _config.MaxCount, StickmanCount)); 

            newPos.x = Mathf.Clamp(newPos.x, -limit, limit);
        }


        void IStickmanController.Stop()
        {
            _characters.ForEach(s => s.PlayStop()); 
        }
    }
}