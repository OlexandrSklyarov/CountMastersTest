using System;
using UnityEngine;
using Data.DataStruct;
using Source.Gameplay.Characters;

namespace Source.Data
{
    [Serializable]
    public sealed class EnemyData
    {
        [field: SerializeField] public StickmanType StickmanType {get; private set;} = StickmanType.SIMPLE_RED;
        [field: Space(10f), SerializeField] public RangeIntValue SpawnCount {get; private set;} = new RangeIntValue(20, 50);
        [field: Space(10f), SerializeField, Min(0.1f)] public float UnitsSpeed {get; private set;} = 1f;
        [field: SerializeField, Min(0.1f)] public float UnitsRotationSpeed {get; private set;} = 5f;  
        [field: Space(10f), SerializeField] public UnitFormationData Formation {get; private set;}
    }
}