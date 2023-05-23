using Services.Pooling;
using Source.Data;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
    public class Stickman : MonoBehaviour
    {
        private IFactoryStorage<Stickman> _storage;
        private Animator _animator;
        private int _runState;
        private bool _isInit;


        public void Init(IFactoryStorage<Stickman> storage)
        {
            _storage = storage;

            if (!_isInit)
            {
                _animator = GetComponent<Animator>();
                _runState = Animator.StringToHash(ConstPrm.Animator.RUN);
                _isInit = true;
            }
        }


        public void PlayRun() => _animator.SetBool(_runState, true);

        
        public void PlayStop() => _animator.SetBool(_runState, false);
    }


    public enum StickmanType
    {
        SIMPLE_STICKMAN = 0
    }
}