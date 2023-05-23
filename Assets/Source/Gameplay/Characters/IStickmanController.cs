using System;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    public interface IStickmanController
    {
        Transform Transform {get;}

        event Action FailureEvent;

        void Move(Vector3 vector3);
        void Stop();
    }
}