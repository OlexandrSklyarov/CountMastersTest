using UnityEngine;
using Source.Gameplay.Environment;

namespace Source.Gameplay
{
    public class GameStartup : MonoBehaviour
    {        
        [SerializeField] private RoadController _roadController;

        private GameProcess _gameProcess;
        private bool _isRun;


        private void Start()
        {
            _roadController.Init();

            _gameProcess = new GameProcess(_roadController);
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