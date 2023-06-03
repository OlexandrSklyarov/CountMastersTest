using System;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public class SceneData
    {
        [field: SerializeField] public string[] Levels {get; private set;}        
    }
}