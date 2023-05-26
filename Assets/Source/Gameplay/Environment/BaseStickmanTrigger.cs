using UnityEngine;

namespace Source.Gameplay.Environment
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public abstract class BaseStickmanTrigger : MonoBehaviour
    {
        private void Awake() 
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }


        private void OnTriggerEnter(Collider other) => Interact(other);
       

        protected abstract void Interact(Collider other);
    }
}