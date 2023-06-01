using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Source.Gameplay.UI
{    
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private Button _confirmBtn;
        
        private bool _isConfirm;


        public void Init() 
        {
            _confirmBtn.onClick.AddListener(OnConfirm);
        }


        private void OnConfirm() => _isConfirm = true;


        private void OnDestroy() 
        {
            OnConfirm();
        }


        public async Task WaitConfirmAsync()
        {
            while(!_isConfirm) await Task.Yield();
        }
    }
}