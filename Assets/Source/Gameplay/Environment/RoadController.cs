using UnityEngine;

namespace Source.Gameplay.Environment
{
    public class RoadController : MonoBehaviour, IRoad
    {
        [SerializeField] private Transform _road;

        private Transform _tr;

        public void Init()
        {
            _tr = transform;
        }


        void IRoad.Move(float speed)
        {
            _road.Translate(-_tr.forward * speed * Time.deltaTime);
        }
    }
}