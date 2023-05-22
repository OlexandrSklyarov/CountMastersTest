using System;
using Source.Gameplay.Characters;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public sealed class StickmanData
    {
        [field: SerializeField] public StickmanType Type { get; private set; } = StickmanType.SIMPLE_STICKMAN;
        [field: SerializeField] public Stickman Prefab{get; private set;}
        [field: SerializeField, Min(64)] public int PoolSize{get; private set;}
    }
}