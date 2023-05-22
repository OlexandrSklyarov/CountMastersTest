using Services.Pooling;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Stickman : MonoBehaviour
    {
        private IFactoryStorage<Stickman> _storage;


        public void Init(IFactoryStorage<Stickman> storage)
        {
            _storage = storage;
        }
    }


    public enum StickmanType
    {
        SIMPLE_STICKMAN = 0
    }
}