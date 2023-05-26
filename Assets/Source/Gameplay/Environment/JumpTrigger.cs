using Source.Gameplay.Characters;
using UnityEngine;

namespace Source.Gameplay.Environment
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class JumpTrigger : MonoBehaviour
    {
        private void Awake() 
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IJump item))
            {
                item.ActiveJump();
            }    
        }
    }
}