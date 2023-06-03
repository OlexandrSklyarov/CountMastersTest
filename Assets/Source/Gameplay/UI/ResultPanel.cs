using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Source.Gameplay.UI
{    
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private Button _confirmBtn;
        
        private TaskCompletionSource<bool> _tcs;


        public void Init() 
        {
            _confirmBtn.onClick.AddListener(OnConfirm);
        }


        private void OnConfirm() => _tcs?.SetResult(true);


        private void OnDestroy() => _tcs?.TrySetCanceled();


        public async Task<bool> WaitConfirmAsync()
        {
            _tcs = new TaskCompletionSource<bool>();
            return await _tcs.Task;
        }
    }
}