using System.Collections.Generic;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    public interface IAttackerGroup
    {
        bool IsAlive {get;}
        Vector3 Center {get;}
        List<Stickman> Units {get;}
    }
}