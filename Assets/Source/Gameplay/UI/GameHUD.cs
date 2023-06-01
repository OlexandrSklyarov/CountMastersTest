using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Source.Gameplay.UI
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private ResultPanel _start;
        [SerializeField] private ResultPanel _win;
        [SerializeField] private ResultPanel _loss;
        [SerializeField] private GamePanel _game;


        public void Init()
        {
            _start.Init();
            _game.Init();
            _win.Init();
            _loss.Init();

            Hide(_game);
            Hide(_win);
            Hide(_loss);
        }


        public void ActiveGamePanel() 
        {
            Hide(_start);
            Show(_game);
        }

        public async Task WaitStartGameAsync()
        {
            await _start.WaitConfirmAsync();

            Hide(_start);
            Show(_game);
        }


        public async Task WinConfirmAsync()
        {
            Hide(_game);
            Show(_win);

            await _win.WaitConfirmAsync();
        }


        public async Task LossConfirmAsync()
        {
            Hide(_game);
            Show(_loss);
            
            await _loss.WaitConfirmAsync();
        }


        private void Show(Component comp) => comp.gameObject.SetActive(true);
        private void Hide(Component comp) => comp.gameObject.SetActive(false);
    }
}
