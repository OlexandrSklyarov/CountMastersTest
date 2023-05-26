using System;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    public interface IStickmanController
    {
        Transform Transform {get;}

        event Action FailureEvent;

        void OnUpdate();
        void Move(float x, float z);
        void Stop();
    }
}