using UnityEngine;

namespace Source.Gameplay.Environment
{
    public class Rotater : MonoBehaviour
    {              
        [SerializeField, Min(0.1f)] private float _speed = 3f;
        [SerializeField] bool _isInvertRotation;

        private Transform _tr;
        

        private void Awake() 
        {
            _tr = transform;
            _tr.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }

       
        private void Update()
        {
            _tr.Rotate(_tr.up * _speed * ((_isInvertRotation)? -1f : 1f) * Time.deltaTime, Space.Self);
        }
    }
}