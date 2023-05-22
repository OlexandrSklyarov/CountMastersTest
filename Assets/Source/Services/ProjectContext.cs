using Source.Data;
using UnityEngine;

namespace Source.Services
{
    public class ProjectContext : MonoBehaviour
    {
        public static ProjectContext Instance => _instance;
        public MainConfig MainConfig => _mainConfig;

        [SerializeField] private MainConfig _mainConfig;
        
        private static ProjectContext _instance;

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }


        public void Init()
        {

        }        
    }
}