using UnityEngine;
using Cinemachine;
using Source.Gameplay.Characters.Enemy;
using Source.Gameplay.UI;
using DG.Tweening;

namespace Source.Gameplay
{
    public class GameStartup : MonoBehaviour
    {        
        private enum GameState {WAIT, RUN}

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private EnemyContainer _enemyContainer;
        [SerializeField] private GameHUD _ui;

        private GameProcess _gameProcess;
        private GameState _state;


        private void Start()
        {
            _ui.Init();

            DOTween.SetTweensCapacity(500, 100);

            _gameProcess = new GameProcess(_playerSpawnPoint, _enemyContainer, _camera);
            _gameProcess.CompletedEvent += OnCompleted;
            _gameProcess.FailureEvent += OnFailure;

            WaitPressPlayAsync();
        }


        private void SetRunStatus(GameState s) => _state = s;


        private async void WaitPressPlayAsync()
        {            
            await _ui.WaitStartGameAsync();
            
            _gameProcess.Play();
            SetRunStatus(GameState.RUN);
        }


        private async void OnCompleted(int point)
        {
            _gameProcess.Stop();  
            SetRunStatus(GameState.WAIT); 

            Debug.Log("Completed"); 

            await _ui.WinConfirmAsync(); 

            Debug.Log("Next");      
        }


        private async void OnFailure()
        {
            _gameProcess.Stop();  
            SetRunStatus(GameState.WAIT); 
            Debug.Log("Failure..."); 

            await _ui.LossConfirmAsync(); 

            Debug.Log("RESTART"); 
        }


        private void Update()
        {
            if (_state == GameState.WAIT) return;

            _gameProcess?.OnUpdate();
        }
    }
}