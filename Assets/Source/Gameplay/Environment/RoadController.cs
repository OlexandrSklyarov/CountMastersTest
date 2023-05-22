using UnityEngine;

namespace Source.Gameplay.Environment
{
    public class RoadController : MonoBehaviour, IRoad
    {
        private Transform _tr;

        public void Init()
        {
            _tr = transform;
        }


        void IRoad.Move(float speed)
        {
            _tr.Translate(-_tr.forward * speed * Time.deltaTime);
        }
    }
}