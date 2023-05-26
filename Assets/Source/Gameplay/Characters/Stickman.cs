using Services.Pooling;
using Source.Data;
using UnityEngine;
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
    public class Stickman : MonoBehaviour, IDamage, IJump
    {
        public StickmanType Type => _myType;
        public bool IsAlive {get; private set;}

        [SerializeField] private StickmanType _myType;

        private IFactoryStorage<Stickman> _storage;
        private Transform _tr;
        private Animator _animator;
        private int _runState;
        private bool _isInit;
        private Sequence _tween;

        public event Action<Stickman> DieEvent;
        public event Action<Stickman> JumpEvent;


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
            _tween?.Kill();
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


        void IJump.ActiveJump() => JumpEvent?.Invoke(this);


        public void Jump(float duration)
        {
            _tween = _tr.DOLocalJump(_tr.localPosition, 3f, 1, duration);
        }


        public void SetLocalPositionAndRotation(Vector3 pos, Vector3 rot, float duration)
        {
            _tween?.Kill();
            _tween = DOTween.Sequence();
            _tween.Append(_tr.DOLocalMove(pos, duration).SetEase(Ease.OutBack));
            _tween.Append(_tr.DOLocalRotate(rot, duration).SetEase(Ease.OutBack));
        }
    }


    public enum StickmanType
    {
        SIMPLE_STICKMAN = 0,
        SIMPLE_RED
    }
}