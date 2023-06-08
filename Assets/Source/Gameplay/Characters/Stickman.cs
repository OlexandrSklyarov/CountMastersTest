using Services.Pooling;
using Source.Data;
using UnityEngine;
using System;
using DG.Tweening;
using Source.Gameplay.Environment;

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
        private Sequence _tween;
        private bool _isInit;
        private bool _isFinished;

        public event Action<Stickman> DieEvent;
        public event Action<Stickman, int> FinishEvent;
        public event Action<Stickman, float> JumpEvent;


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

            if (other.TryGetComponent(out IFinishBlock block))
            {
                StopSequence();
                _isFinished = true;
                _tr.SetParent(null);
                
                FinishEvent?.Invoke(this, block.Points);

            }
        }


        private bool IsEnemyAndAlive(Stickman other)
        {
            return _myType != other.Type && other.IsAlive;
        }


        void IJump.ActiveJump(float jumpPower) => JumpEvent?.Invoke(this, jumpPower);


        public void Jump(float jumpPower, float duration)
        {
            _tween = _tr.DOLocalJump(_tr.localPosition, jumpPower, 1, duration);
        }


        public void Release()
        {
            _tr.SetParent(null);
            PlayStop();
        }


        public void SetLocalPositionAndRotation(Vector3 pos, Vector3 rot, float duration)
        {
            StopSequence();

            if (_isFinished) return;

            _tween = DOTween.Sequence();
            _tween.Append(_tr.DOLocalMove(pos, duration).SetEase(Ease.OutBack));
            _tween.Append(_tr.DOLocalRotate(rot, duration).SetEase(Ease.OutBack));
        }
        
        
        private void StopSequence() => _tween?.Kill();
    }




    public enum StickmanType
    {
        SIMPLE_STICKMAN = 0,
        SIMPLE_RED
    }
}