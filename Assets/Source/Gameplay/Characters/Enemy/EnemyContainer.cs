using System;
using Source.Data;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay.Characters.Enemy
{
    public class EnemyContainer : MonoBehaviour
    {
        private EnemyGroupController[] _unitGroups;

        public void Init(EnemyData enemyConfig, StickmanFactory factory)
        {
            _unitGroups = GetComponentsInChildren<EnemyGroupController>();

            Array.ForEach(_unitGroups, g => g.Init(enemyConfig, factory));
        }


        public void Stop()
        {
            Array.ForEach(_unitGroups, g => g.Stop());
        }


        public void OnUpdate()
        {
            Array.ForEach(_unitGroups, g => g.OnUpdate());
        }
    }
}