using System;
using Cinemachine;
using Common.Input;
using Source.Data;
using Source.Gameplay.Characters;
using Source.Gameplay.Characters.Enemy;
using Source.Gameplay.Player;
using Source.Services;
using UnityEngine;

namespace Source.Gameplay
{
    public class GameProcess
    {
        private readonly MainConfig _config;
        private readonly EnemyContainer _enemyContainer;
        private readonly TouchInputManager _input;
        private readonly PlayerController _playerController;

        public event Action CompletedEvent;            
        public event Action FailureEvent;           


        public GameProcess(Transform _playerSpawnPoint, Characters.Enemy.EnemyContainer enemyContainer, 
            CinemachineVirtualCamera camera)
        {
            _config = ProjectContext.Instance.MainConfig;
            _enemyContainer = enemyContainer;

            _input = new TouchInputManager();

            var stickmanFactory = new StickmanFactory(_config.StickmanCollection);

            _enemyContainer.Init(_config.EnemyConfig, stickmanFactory);

            var stickmanController = GetStickmanController(_playerSpawnPoint, stickmanFactory);
            
            _playerController = new PlayerController(_config.PlayerConfig, stickmanController, _input);
            _playerController.SuccessEvent += OnCompleted;
            _playerController.FailureEvent += OnFailure;

            SetupCamera(camera, stickmanController.transform);
        }
        

        private void SetupCamera(CinemachineVirtualCamera camera, Transform target)
        {
            camera.Follow = target;
        }


        private StickmanController GetStickmanController(Transform spawnPoint, StickmanFactory factory)
        {
            var controller = UnityEngine.Object.Instantiate(
                _config.StickmanControllerPrefab, spawnPoint.position, spawnPoint.rotation);
            
            controller.Init(_config.StickmenControllerConfig, factory);
                
            return controller;
        }


        public void OnUpdate()
        {
            _input?.OnUpdate();
            _playerController?.OnUpdate();
            _enemyContainer?.OnUpdate();
        }


        public void Play()
        {
            _input?.OnEnable();
            _playerController?.Enable();
        }


        public void Stop()
        {
            _input?.OnDisable();
            _playerController?.Disable();
            _enemyContainer?.Stop();
        }


        private void OnFailure()
        {
            UnScribePlayerController();
            FailureEvent?.Invoke();
        }
        

        private void OnCompleted()
        {
            UnScribePlayerController();
            CompletedEvent?.Invoke();
        }


        private void UnScribePlayerController()
        {
            _playerController.SuccessEvent -= OnCompleted;
            _playerController.FailureEvent -= OnFailure;
        }
    }
}