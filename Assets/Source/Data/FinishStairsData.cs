using UnityEngine;
using Source.Gameplay.Environment;
using System;

namespace Source.Data
{
    [CreateAssetMenu(menuName = "SO/FinishStairsData", fileName = "FinishStairsConfig")]
    public class FinishStairsData : ScriptableObject
    {
        [field: SerializeField] public StairBlock BlockPrefab {get; private set;}
        [field: SerializeField, Min(1)] public int MinBlocksAmount {get; private set;} = 10;
        [field: SerializeField, Min(1)] public int PointsStep {get; private set;} = 10;
        [field: SerializeField] public StairColor[] BlockColors {get; private set;}
        [field: SerializeField] public float Y_Offset {get; private set;} = 2f;
        [field: SerializeField] public float Z_Offset {get; private set;} = 2f;

        [Serializable]
        public struct StairColor
        {
            [field: SerializeField] public Color Base {get; private set;}
            [field: SerializeField, ColorUsage(true, true)] public Color Emission {get; private set;}
        }
    }
}