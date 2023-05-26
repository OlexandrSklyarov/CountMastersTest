using Source.Gameplay.Characters;
using UnityEngine;

namespace Source.Gameplay.Environment
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class JumpTrigger : MonoBehaviour
    {
        [SerializeField, Range(1f, 8f)] private float _jumpPower = 4f;
        private void Awake() 
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IJump item))
            {
                item.ActiveJump(_jumpPower);
            }    
        }
    }
}