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
        [field: Space(10f), SerializeField] public MovementConfig Movement {get; private set;}
        [field: Space(10f), SerializeField] public UnitConfig Unit {get; private set;}
        [field: Space(10f), SerializeField] public UnitFormationData Formation {get; private set;}
        [field: Space(10f), SerializeField] public TowerData Tower {get; private set;}


        [Serializable]
        public class UnitConfig
        {
            [field: SerializeField, Min(0.1f)] public float DistanceFactor {get; private set;} = 0.5f;
            [field: SerializeField, Min(0.1f)] public float Radius {get; private set;} = 0.3f;
            [field: SerializeField, Min(0.1f)] public float AttackSpeed {get; private set;} = 0.2f;
            [field: SerializeField, Min(0.1f)] public float AttackRotateSpeed {get; private set;} = 5f;
            [field: SerializeField, Min(0.1f)] public float ReturnToOriginDuration {get; private set;} = 3f;
        }


        [Serializable]
        public class MovementConfig
        {
            [field: SerializeField, Min(1f)] public float VerticalSpeed {get; private set;} = 12f;
            [field: SerializeField, Min(1f)] public float HorizontalSpeed {get; private set;} = 2f;
            [field: SerializeField, Min(1f)] public float HorizontalMovementLimit {get; private set;} = 7f;
            [field: SerializeField, Min(0.1f)] public float AttackMoveSpeed {get; private set;} = 2f;
            [field: SerializeField, Min(0.1f)] public float JumpDuration {get; private set;} = 2f;
        }
    }
}