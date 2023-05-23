using System;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public sealed class PlayerData
    {
        [field: SerializeField] public MovementData Movement {get; private set;}
        
        [Serializable]
        public sealed class MovementData
        {
            [field: SerializeField, Min(1f)] public float VerticalSpeed {get; private set;} = 2f;
            [field: SerializeField, Min(1f)] public float HorizontalSpeed {get; private set;} = 5f;
        }
    }
}