using System;
using DG.Tweening;
using Source.Data;
using Source.Gameplay.Environment;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class StickmanController : MonoBehaviour, 
        IStickmanController, IStickmanChange, IInteractTarget
    {
        private int StickmanCount => _container.childCount;

        [SerializeField] private Transform _container;
        [SerializeField] private StickmenViewInfo _counter;
        private StickmanControllerData _config;
        private StickmanFactory _factory;
        private Transform _tr;

        public event Action<int, int> ChangeStickmanCountEvent;


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
            }

            Formation();

            ChangeStickmanCountEvent?.Invoke(prev, StickmanCount);
        }


        private void Formation()
        {
            for (int i = 1; i < _container.childCount; i++)
            {
                var newPos = new Vector3
                (
                    _config.UnitDistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _config.UnitRadius),
                    0f,
                    _config.UnitDistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _config.UnitRadius)
                );

                _container.GetChild(i).transform
                    .DOLocalMove(newPos, 1f)
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
    }
}