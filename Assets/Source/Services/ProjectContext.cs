using Source.Data;
using UnityEngine;

namespace Source.Services
{
    public class ProjectContext : MonoBehaviour
    {
        public MainConfig MainConfig => _mainConfig;
        public SceneController SceneController {get; private set;}

        public static ProjectContext Instance => _instance;

        [SerializeField] private MainConfig _mainConfig;
        
        private static ProjectContext _instance;

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }


        public void Init()
        {
            SceneController = new SceneController(_mainConfig.SceneConfig);
        }        
    }
}