using UnityEngine;

namespace Source.Data
{
    [CreateAssetMenu(menuName = "SO/UnitFormationData", fileName = "UnitFormationConfig")]
    public class UnitFormationData : ScriptableObject
    {
        [field: Space(10f), SerializeField, Min(0.1f)] public float UnitDistanceFactor {get; private set;} = 0.35f;
        [field: SerializeField, Min(0.1f)] public float UnitRadius {get; private set;} = 1f;
    }
}