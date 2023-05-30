using System;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public sealed class TowerData
    {
        [field: SerializeField, Min(1)] public int MaxUnitPerRow { get; private set; } = 8;
        [field: SerializeField, Min(0.01f)] public float X_Offset { get; private set; } = 0.3f;     
        [field: SerializeField, Min(0.01f)] public float Y_Offset { get; private set; } = 0.3f;
        [field: SerializeField, Range(2, 16)] public int MaxCountInRow { get; private set; } = 5;
    }
}