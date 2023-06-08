using UnityEngine;
using Source.Data;
using Cinemachine;

namespace Source.Gameplay.GameCamera
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private CinemachineVirtualCamera _processCamera;
        [SerializeField] private CinemachineVirtualCamera _resultCamera;
        

        public void Init(CameraData config)
        {
            SetupCamera(_processCamera, config.GameCameraOffset);
            SetupCamera(_resultCamera, config.ResultCameraOffset);
        }


        void ICameraController.SetState(CameraState state)
        {
            _processCamera.Priority = (state == CameraState.GAME) ? 10 : 0;
            _resultCamera.Priority = (state == CameraState.RESULT) ? 10 : 0;
        }


        void ICameraController.SetTarget(Transform target)
        {
            _processCamera.Follow = target;
            
            _resultCamera.Follow = target;
            _resultCamera.LookAt = target;
        }


        private void SetupCamera(CinemachineVirtualCamera camera, Vector3 offset)
        {            
            var transposer = camera.GetCinemachineComponent<CinemachineTransposer>();            
            transposer.m_FollowOffset = offset;
        }
    }
}