using UnityEngine;
using UnityEngine.SceneManagement;
using Source.Data;

namespace Source.Services
{
    public class SceneController
    {       
        private string CurrentSceneName => SceneManager.GetActiveScene().name;

        private readonly SceneData _config;
        private int _levelIndex;


        public SceneController(SceneData config) 
        {
            _config = config;
            _levelIndex = -1;
        }


        public void RestartCurrentLevel()
        {
            Load(CurrentSceneName);
        }


        public void LoadNextLevel()
        {
            _levelIndex = ++_levelIndex % _config.Levels.Length;
            Load(_config.Levels[_levelIndex]);
        }


        private void Load(string sceneName) => SceneManager.LoadSceneAsync(sceneName);
    }
}