using System;
using Cinemachine;
using Source.Data;
using Source.Gameplay.Characters;
using Source.Gameplay.Environment;
using Source.Gameplay.Player;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay
{
    public class GameProcess
    {
        private readonly MainConfig _config;
        private readonly IRoad _road;
        private readonly PlayerController _playerController;

        public event Action CompletedEvent;            
        public event Action FailureEvent;           


        public GameProcess(IRoad road, Transform _playerSpawnPoint, CinemachineVirtualCamera camera)
        {
            _config = ProjectContext.Instance.MainConfig;
            _road = road;

            var stickmanController = GetStickmanController(_playerSpawnPoint);
            _playerController = new PlayerController(_config.PlayerConfig, _road, stickmanController);

            SetupCamera(camera, stickmanController.transform);
        }


        private void SetupCamera(CinemachineVirtualCamera camera, Transform target)
        {
            camera.Follow = target;
        }


        private StickmanController GetStickmanController(Transform spawnPoint)
        {
            var controller = UnityEngine.Object.Instantiate(
                _config.StickmanControllerPrefab, spawnPoint.position, spawnPoint.rotation);

            var factory = new StickmanFactory(_config.StickmanCollection);
            controller.Init(_config.StickmenControllerConfig, factory);
                
            return controller;
        }


        public void OnUpdate()
        {
            
        }


        public void Play()
        {
            
        }


        public void Stop()
        {
            
        }
    }
}