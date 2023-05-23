using UnityEngine;

namespace Source.Gameplay.Characters
{
    public interface IStickmanController
    {
        Transform Transform {get;}

        void Move(Vector3 vector3);
        void Stop();
    }
}