using UnityEngine;
using DG.Tweening;

namespace Source.Services
{
    public class AppStartup : MonoBehaviour
    {    
        void Start()
        {
            ProjectContext.Instance.Init();

            DOTween.SetTweensCapacity(500, 500);

            ProjectContext.Instance.SceneController.LoadNextLevel();
        }    
    }
}