using System;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public class StickmanControllerData
    {
        [field: SerializeField, Min(1)] public int StartStickmanCount {get; private set;} = 3;        
        [field: SerializeField, Min(1)] public int MinCount {get; private set;} = 3;        
        [field: SerializeField, Min(1)] public int MaxCount {get; private set;} = 250;        
        [field: SerializeField, Min(1f)] public float HorizontalMovementLimit {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float UnitDistanceFactor {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float UnitRadius {get; private set;} = 0.3f;
        [field: SerializeField, Min(0.1f)] public float UnitAttackSpeed {get; private set;} = 0.2f;
        [field: SerializeField, Min(0.1f)] public float UnitAttackRotateSpeed {get; private set;} = 5f;
    }
}