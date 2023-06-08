using System;
using UnityEngine;

namespace Source.Data
{
    [Serializable]
    public class CameraData
    {
        [field: SerializeField] public Vector3 GameCameraOffset {get; private set;}
        [field: SerializeField] public Vector3 ResultCameraOffset {get; private set;}
    }
}