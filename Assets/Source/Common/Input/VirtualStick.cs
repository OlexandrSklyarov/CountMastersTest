using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Input
{
    
    public class VirtualStick : MonoBehaviour
    {
        [SerializeField] private Image[] _stickImages;
        [SerializeField] private bool _isHideView;

        private RectTransform _myRectTransform;
        private Vector2 _offset;

        private const float OFFSET_RANGE = 0.5f;

        public void Init(Vector2 referenceResolution)
        {
            _myRectTransform = GetComponent<RectTransform>();
            _offset = new Vector2()
            {
                x = _myRectTransform.rect.width * OFFSET_RANGE * (Screen.width / referenceResolution.x),
                y = _myRectTransform.rect.height * OFFSET_RANGE * (Screen.height / referenceResolution.y)
            };

            Show();
            EnableView();
        }


        private void EnableView()
        {
            Array.ForEach(_stickImages, img =>
            {
                if (_isHideView)
                {
                    var c = img.color;
                    c.a = 0f;
                    img.color = c;
                }
            });
        }
        

        public void Show() => EnabledImages(true);


        public void Hide() => EnabledImages(false);


        public void SetScreenPosition(Vector2 screenPosition)
        {
            _myRectTransform.position = screenPosition - _offset;
        }


        private void EnabledImages(bool isActive)
        {
            foreach (var image in _stickImages)
            {
                image.enabled = isActive;
            }
        }
    }
}