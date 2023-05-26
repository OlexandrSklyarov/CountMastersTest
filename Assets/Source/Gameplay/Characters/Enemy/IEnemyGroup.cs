using System;
using System.Collections;
using UnityEngine;

namespace Source.Gameplay.Characters.Enemy
{
    public interface IEnemyGroup
    {
        Vector3 Center {get;}
        bool IsAlive { get; }

        event Action<IEnemyGroup> KillAllUnitsEvent;
        void PrepareForAttack(IAttackerGroup group);
        void Attack(Vector3 attackPosition);
        void StopAttack();
    }
}