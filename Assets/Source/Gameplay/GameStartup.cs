using UnityEngine;
using Source.Gameplay.Environment;
using Cinemachine;

namespace Source.Gameplay
{
    public class GameStartup : MonoBehaviour
    {        
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private CinemachineVirtualCamera _camera;

        private GameProcess _gameProcess;
        private bool _isRun;


        private void Start()
        {
            _gameProcess = new GameProcess(_playerSpawnPoint, _camera);
            _gameProcess.CompletedEvent += OnCompleted;
            _gameProcess.FailureEvent += OnFailure;

            _gameProcess.Play();
            SetRunStatus(true);
        }


        private void SetRunStatus(bool status) => _isRun = status;


        private void OnCompleted()
        {
            _gameProcess.Stop();  
            SetRunStatus(false);          
        }


        private void OnFailure()
        {
            _gameProcess.Stop();  
            SetRunStatus(false); 
        }


        private void Update()
        {
            if (!_isRun) return;

            _gameProcess?.OnUpdate();
        }
    }
}