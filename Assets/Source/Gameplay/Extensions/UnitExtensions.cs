using UnityEngine;

namespace Source.Gameplay.Extensions
{
    public static class UnitExtensions
    {
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