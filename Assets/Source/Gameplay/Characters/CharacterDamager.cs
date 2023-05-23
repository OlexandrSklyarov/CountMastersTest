using UnityEngine;

namespace Source.Gameplay.Characters
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class CharacterDamager : MonoBehaviour
    {
        private void Awake() 
        {
            GetComponent<BoxCollider>().isTrigger = true;            
            GetComponent<Rigidbody>().isKinematic = true;            
        }


        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent(out IDamage item))
            {
                item.ApplyDamage();
            }
        }
        
    }
}