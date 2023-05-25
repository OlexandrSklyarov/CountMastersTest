using System;
using System.Collections;
using UnityEngine;

namespace Source.Gameplay.Characters.Enemy
{
    public interface IEnemyGroup
    {
        Vector3 Center {get;}        
        event Action<IEnemyGroup> DestroyEvent;
        void SendAttack(IAttackerGroup group);
    }
}