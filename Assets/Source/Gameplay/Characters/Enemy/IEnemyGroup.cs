using System;

namespace Source.Gameplay.Characters.Enemy
{
    public interface IEnemyGroup
    {
        event Action<IEnemyGroup> DestroyEvent;

        void SendAttack(IAttackGroup group);
    }
}