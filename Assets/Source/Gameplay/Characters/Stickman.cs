using Services.Pooling;
using Source.Data;
using UnityEngine;
using System;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
    public class Stickman : MonoBehaviour, IDamage
    {
        public StickmanType Type => _myType;
        public bool IsAlive {get; private set;}

        [SerializeField] private StickmanType _myType;

        private IFactoryStorage<Stickman> _storage;
        private Transform _tr;
        private Animator _animator;
        private int _runState;
        private bool _isInit;

        public event Action<Stickman> DieEvent;


        public void Init(IFactoryStorage<Stickman> storage)
        {
            _storage = storage;
            IsAlive = true;

            if (!_isInit)
            {
                _tr = transform;
                _animator = GetComponent<Animator>();
                _runState = Animator.StringToHash(ConstPrm.Animator.RUN);
                _isInit = true;
            }
        }


        public void PlayRun() => _animator.SetBool(_runState, true);

        
        public void PlayStop() => _animator.SetBool(_runState, false);


        public void MoveToPosition(Vector3 pos, float speed, float rotationSpeed)
        {
            _tr.rotation = Quaternion.RotateTowards
            (
                _tr.rotation,
                Quaternion.LookRotation(pos - _tr.position, Vector3.up),
                rotationSpeed * Time.deltaTime
            );

            _tr.position = Vector3.Lerp(_tr.position, pos, speed * Time.deltaTime);
        }


        void IDamage.ApplyDamage() => Die();


        public void ApplyHit() => Die();


        public void Die()
        {
            IsAlive = false;
            DieEvent?.Invoke(this);
            _storage.ReturnToStorage(this);
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out Stickman stickman) && IsEnemyAndAlive(stickman))
            {
                stickman.Die();
                Die();
            }
        }


        private bool IsEnemyAndAlive(Stickman other)
        {
            return _myType != other.Type && other.IsAlive;
        }
    }


    public enum StickmanType
    {
        SIMPLE_STICKMAN = 0,
        SIMPLE_RED
    }
}