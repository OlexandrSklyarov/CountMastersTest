using UnityEngine;

namespace Source.Gameplay.Characters
{
    public interface IAttackGroup
    {
        bool IsAlive {get;}
        Vector3 Center {get;}
    }
}