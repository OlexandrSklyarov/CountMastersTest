using Source.Gameplay.Characters;
using UnityEngine;

namespace Source.Gameplay.Environment
{    
    public class JumpTrigger : BaseStickmanTrigger
    {
        [SerializeField, Range(1f, 8f)] private float _jumpPower = 4f;

        protected override void Interact(Collider other) 
        {
            if (other.TryGetComponent(out IJump item))
            {
                item.ActiveJump(_jumpPower);
            }    
        }
    }
}