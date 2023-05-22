using System;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public class StickmanControllerData
    {
        [field: SerializeField, Min(0.1f)] public float MoveSpeed {get; private set;} = 5f;
        [field: SerializeField, Min(1)] public int StartStickmanCount {get; private set;} = 3;        
        [field: SerializeField, Min(1f)] public float HorizontalMovementLimit {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float UnitDistanceFactor {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float UnitRadius {get; private set;} = 0.3f;
    }
}