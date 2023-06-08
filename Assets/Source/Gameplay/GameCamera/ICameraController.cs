
using UnityEngine;

namespace Source.Gameplay.GameCamera
{
    public interface ICameraController
    {
        void SetTarget(Transform transform);
        void SetState(CameraState state);
    }
}