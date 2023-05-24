using System;
using Source.Data;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay.Characters.Enemy
{
    public class EnemyGroup : MonoBehaviour, IEnemyGroup
    {
        public event Action<IEnemyGroup> DestroyEvent;
        

        public void Init(EnemyData enemyConfig, StickmanFactory factory)
        {
        }


        public void Stop()
        {
            throw new NotImplementedException();
        }
        

        void IEnemyGroup.SendAttack(IAttackGroup group)
        {
            
        }
    }
}