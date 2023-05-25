using System.Collections.Generic;
using UnityEngine;

namespace Source.Gameplay.Extensions
{
   

    public static class UnitExtensions
    {
        public static class Formation
        {
            public static IEnumerable<Vector3> Box(int unitWidth, int unitDepth, UnitFormation.BoxSettings s)
            {
                return Box(unitWidth, unitDepth, s.NthOffset, s.Noise, s.Spread, s.Hollow);
            }


            public static IEnumerable<Vector3> Box(int unitWidth, int unitDepth,
                float nthOffset, float noise, float spread, bool hollow)
            {
                var middleOffset = new Vector3(unitWidth * 0.5f, 0, unitDepth * 0.5f);

                for (var x = 0; x < unitWidth; x++)
                {
                    for (var z = 0; z < unitDepth; z++)
                    {
                        if (hollow && x != 0 && x != unitWidth - 1 && z != 0 && z != unitDepth - 1) continue;
                        var pos = new Vector3(x + (z % 2 == 0 ? 0 : nthOffset), 0, z);

                        pos -= middleOffset;

                        pos += GetNoise(pos, noise);

                        pos *= spread;

                        yield return pos;
                    }
                }
            }


            public static IEnumerable<Vector3> Radial(int amount, UnitFormation.RadialSettings s)
            {
                return Radial(amount, s.Rings, s.Rotations, s.NthOffset, s.Radius,
                    s.RadiusGrowthMultiplier, s.Noise, s.Spread, s.RingOffset);
            }


            public static IEnumerable<Vector3> Radial(int amount, int rings, float rotations, float nthOffset,
                float radius, float radiusGrowthMultiplier, float noise, float spread, float ringOffset)
            {
                var amountPerRing = amount / rings;
                var ringOffsetResult = 0f;
                for (var i = 0; i < rings; i++)
                {
                    for (var j = 0; j < amountPerRing; j++)
                    {
                        var angle = j * Mathf.PI * (2 * rotations) / amountPerRing + (i % 2 != 0 ? nthOffset : 0);

                        var radiusResult = radius + ringOffsetResult + j * radiusGrowthMultiplier;
                        var x = Mathf.Cos(angle) * radiusResult;
                        var z = Mathf.Sin(angle) * radiusResult;

                        var pos = new Vector3(x, 0, z);

                        pos += GetNoise(pos, noise);

                        pos *= spread;

                        yield return pos;
                    }

                    ringOffsetResult += ringOffset;
                }
            }


            private static Vector3 GetNoise(Vector3 pos, float noise)
            {
                var noiseResult = Mathf.PerlinNoise(pos.x * noise, pos.z * noise);
                return new Vector3(noiseResult, 0, noiseResult);
            }


            public static Vector3 GetPositionInSpiralFormation(float distanceFactor, float unitRadius, int index)
            {
                index = Mathf.Max(0, index);

                return new Vector3
                (
                    distanceFactor * Mathf.Sqrt(index) * Mathf.Cos(index * unitRadius),
                    0f,
                    distanceFactor * Mathf.Sqrt(index) * Mathf.Sin(index * unitRadius)
                );
            }
        }
    }
}