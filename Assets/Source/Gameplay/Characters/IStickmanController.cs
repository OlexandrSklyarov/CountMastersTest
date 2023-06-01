using System;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    public interface IStickmanController
    {
        Transform Transform {get;}

        event Action FailureEvent;
        event Action<int> CompletedEvent;

        void OnUpdate();
        void Move(float x, float z);
        void Stop();
    }
}