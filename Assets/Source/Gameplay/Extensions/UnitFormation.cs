using System;
using UnityEngine;

namespace Source.Gameplay.Extensions
{
    [Serializable]
    public sealed class UnitFormation
    {
        [Serializable]
        public struct BoxSettings
        {
            [Min(0.01f)] public float NthOffset;
            [Min(0.01f)] public float Noise;
            [Min(0.01f)] public float Spread;
            public bool Hollow;
        }

        [Serializable]
        public struct RadialSettings
        {
            [Min(1)] public int Rings;
            [Min(0.01f)] public float Rotations;
            [Min(0.01f)] public float NthOffset;
            [Min(0.01f)] public float Radius;
            [Min(0.01f)] public float RadiusGrowthMultiplier;
            [Min(0.01f)] public float Noise;
            [Min(0.01f)] public float Spread;
            [Min(0.01f)] public float RingOffset;
        }
    }
}