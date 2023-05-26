using System;
using System.Collections.Generic;
using DG.Tweening;
using Source.Data;
using Source.Gameplay.Extensions;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay.Characters.Enemy
{
    [RequireComponent(typeof(BoxCollider))]
    public class EnemyGroupController : MonoBehaviour, IEnemyGroup, IStickmanInfo
    {
        private enum State { NONE, WAIT, ATTACK, DEATH }

        Vector3 IEnemyGroup.Center => (_container.childCount > 0) ? 
            _container.GetChild(0).position : _tr.position;

        private bool IsCanAttack => _currentState == State.ATTACK && 
            _attackerGroup != null && _attackerGroup.IsAlive;

        private int StickmanCount => _characters.Count;
        bool IEnemyGroup.IsAlive => StickmanCount > 0;

        [SerializeField] private StickmenViewInfo _counter;

        private Transform _tr;
        private IAttackerGroup _attackerGroup;
        private State _currentState;
        private EnemyData _config;
        private StickmanFactory _factory;
        private Transform _container;
        private readonly List<Stickman> _characters = new();

        public event Action<IEnemyGroup> KillAllUnitsEvent;
        public event Action<int, int> ChangeStickmanCountEvent;
        

        public void Init(EnemyData config, StickmanFactory factory)
        {
            _config = config;
            _factory = factory;
            _tr = transform;

            GetComponent<BoxCollider>().isTrigger = true;

            _counter.Init(this);

            _container = new GameObject("Container").transform;
            _container.SetLocalPositionAndRotation(_tr.position, _tr.rotation);
            _container.SetParent(_tr);

            Populate(UnityEngine.Random.Range(_config.SpawnCount.Min, _config.SpawnCount.Max));

            SetState(State.WAIT);
        }


        private void Populate(int num)
        {
            var prev = StickmanCount;

            for (int i = 0; i < num; i++)
            {
                var stickman = _factory.Get(_config.StickmanType);
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
                var newPos = UnitExtensions.Formation
                    .GetPositionInSpiralFormation(_config.Formation.UnitDistanceFactor, _config.Formation.UnitRadius, i);

                var unitTR = _container.GetChild(i).transform;
                unitTR.rotation = Quaternion.Euler(_container.transform.forward * -1f);
                
                unitTR
                    .DOLocalMove(newPos, duration)
                    .SetEase(Ease.OutBack);
            }
        }


        private void SetState(State s) => _currentState = s;


        public void Stop() => SetState(State.NONE);
        

        void IEnemyGroup.PrepareForAttack(IAttackerGroup group)
        {
            if (_currentState == State.NONE) return;
            
            _attackerGroup = group;
            SetState(State.ATTACK);
        }           


        void IEnemyGroup.Attack(Vector3 attackPosition)
        {
            if (!IsCanAttack) return;

            MoveToTarget(attackPosition);
        }


        private void MoveToTarget(Vector3 attackPosition)
        {            
            for (int i = 0; i < StickmanCount; i++)
            {
                var cur = _characters[i];

                cur.MoveToPosition(attackPosition, _config.UnitsSpeed, _config.UnitsRotationSpeed);
                cur.PlayRun();
            }
        }


        void IEnemyGroup.StopAttack()
        {
            SetState(State.WAIT);
            _characters.ForEach(c => c.PlayStop());
        }


        private void OnStickmanDie(Stickman stickman)
        {
            var prev = StickmanCount;
            
            stickman.DieEvent -= OnStickmanDie;
            _characters.Remove(stickman);

            ChangeStickmanCountEvent?.Invoke(prev, StickmanCount);

            if (StickmanCount > 0) return;
            
            _counter.Hide();
            SetState(State.DEATH);
            KillAllUnitsEvent?.Invoke(this);                        
        }
    }
}