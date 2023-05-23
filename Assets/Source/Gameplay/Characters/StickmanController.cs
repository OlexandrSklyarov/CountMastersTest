using System;
using System.Collections.Generic;
using DG.Tweening;
using Source.Data;
using Source.Gameplay.Environment;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class StickmanController : MonoBehaviour, 
        IStickmanController, IStickmanInfo, IInteractTarget
    {
        Transform IStickmanController.Transform => _tr;
        private int StickmanCount => _characters.Count;


        [SerializeField] private Transform _container;
        [SerializeField] private StickmenViewInfo _counter;
        private StickmanControllerData _config;
        private StickmanFactory _factory;
        private Transform _tr;
        private List<Stickman> _characters = new();

        public event Action<int, int> ChangeStickmanCountEvent;
        public event Action FailureEvent;


        public void Init(StickmanControllerData config, StickmanFactory factory)
        {
            _config = config;
            _factory = factory;
            _tr = transform;
            _counter.Init(this);

            Populate(_config.StartStickmanCount);
        }


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


        private void OnStickmanDie(Stickman stickman)
        {
            stickman.DieEvent -= OnStickmanDie;
            _characters.Remove(stickman);

            if (StickmanCount > 0) return;

            FailureEvent?.Invoke();
            _counter.Hide();
        }


        private void Formation(float duration = 1f)
        {
            for (int i = 1; i < StickmanCount; i++)
            {
                var newPos = new Vector3
                (
                    _config.UnitDistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _config.UnitRadius),
                    0f,
                    _config.UnitDistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _config.UnitRadius)
                );

                _container.GetChild(i).transform
                    .DOLocalMove(newPos, duration)
                    .SetEase(Ease.OutBack);
            }
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IInteractable item))   
            {
                item.Interact(this);
            } 
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

            _characters.ForEach(s => s.PlayRun());
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