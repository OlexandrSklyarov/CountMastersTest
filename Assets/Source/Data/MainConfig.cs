using Source.Gameplay.Characters;
using UnityEngine;

namespace Source.Data
{
    [CreateAssetMenu(menuName = "SO/MainConfig", fileName = "MainConfig")]
    public class MainConfig : ScriptableObject
    {
        [field: SerializeField] public PlayerData Player {get; private set;}
        [field: SerializeField] public StickmanController StickmanControllerPrefab{get; private set;}
        [field: Space(20f), SerializeField] public StickmanData[] StickmanCollection{get; private set;}

    }
}