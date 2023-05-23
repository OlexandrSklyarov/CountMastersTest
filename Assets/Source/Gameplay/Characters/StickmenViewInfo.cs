using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Source.Gameplay.Characters
{
    public class StickmenViewInfo : MonoBehaviour
    {
        [SerializeField] TextMeshPro _counter;


        public void Init(IStickmanInfo source)
        {
            source.ChangeStickmanCountEvent += OnChangeHandler;
        }


        private void OnChangeHandler(int prev, int cur)
        {
            DOVirtual.Int(prev, cur, 1f, (v) => _counter.text = $"{v}");
        }
    }
}