using System;
using UnityEngine;
using Source.Gameplay.Characters.Enemy;
using Source.Gameplay.UI;
using Source.Services;
using System.Threading.Tasks;
using Source.Gameplay.GameCamera;

namespace Source.Gameplay
{
    public class GameStartup : MonoBehaviour
    {        
        private enum GameState {WAIT, RUN}

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private CameraController _camera;
        [SerializeField] private EnemyContainer _enemyContainer;
        [SerializeField] private GameHUD _ui;

        private GameProcess _gameProcess;
        private GameState _state;


        private void Start()
        {
            _ui.Init();            

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


        private void OnCompleted(int point)
        {
            _gameProcess.Stop();  
            SetRunStatus(GameState.WAIT);              

            WaitConfirmAsync
            (
                _ui.WinConfirmAsync(),
                () => ProjectContext.Instance.SceneController.LoadNextLevel()
            );             
        }


        private void OnFailure()
        {
            _gameProcess.Stop();  
            SetRunStatus(GameState.WAIT);             

            WaitConfirmAsync
            (
                _ui.LossConfirmAsync(),
                () => ProjectContext.Instance.SceneController.RestartCurrentLevel()
            );
        }


        private async void WaitConfirmAsync(Task<bool> task, Action onConfirm)
        {
            try
            {
                var isConfirm = await task;            
                if (isConfirm) onConfirm?.Invoke();
            }
            catch{}
        }


        private void Update()
        {
            if (_state == GameState.WAIT) return;

            _gameProcess?.OnUpdate();
        }
    }
}