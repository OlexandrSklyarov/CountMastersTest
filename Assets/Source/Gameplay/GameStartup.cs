using UnityEngine;
using Cinemachine;
using Source.Gameplay.Characters.Enemy;
using DG.Tweening;

namespace Source.Gameplay
{
    public class GameStartup : MonoBehaviour
    {        
        private enum GameState {WAIT, RUN}

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private EnemyContainer _enemyContainer;

        private GameProcess _gameProcess;
        private GameState _state;


        private void Start()
        {
            DOTween.SetTweensCapacity(500, 100);

            _gameProcess = new GameProcess(_playerSpawnPoint, _enemyContainer, _camera);
            _gameProcess.CompletedEvent += OnCompleted;
            _gameProcess.FailureEvent += OnFailure;

            _gameProcess.Play();

            SetRunStatus(GameState.RUN);
        }


        private void SetRunStatus(GameState s) => _state = s;


        private void OnCompleted()
        {
            _gameProcess.Stop();  
            SetRunStatus(GameState.WAIT);   
            Debug.Log("Completed");       
        }


        private void OnFailure()
        {
            _gameProcess.Stop();  
            SetRunStatus(GameState.WAIT); 
            Debug.Log("Failure..."); 
        }


        private void Update()
        {
            if (_state == GameState.WAIT) return;

            _gameProcess?.OnUpdate();
        }
    }
}