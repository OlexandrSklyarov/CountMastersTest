using System;
using UnityEngine;

namespace Source.Gameplay.Environment
{
    public class GateHolder : MonoBehaviour
    {
        private Gate[] _gates;

        private void Awake() 
        {
            _gates = GetComponentsInChildren<Gate>();    
            Array.ForEach(_gates, g => g.HitEvent += OnHitHandler);
        }


        private void OnHitHandler()
        {
            Array.ForEach(_gates, g => 
            {
                g.HitEvent -= OnHitHandler;
                g.gameObject.SetActive(false);
            });
        }
    }
}