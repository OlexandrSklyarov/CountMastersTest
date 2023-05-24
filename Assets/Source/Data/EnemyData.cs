using System;
using Data.DataStruct;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public class EnemyData
    {
        [field: SerializeField, Min(1f)] public RangeFloatValue SpawnCount {get; private set;}
    }
}