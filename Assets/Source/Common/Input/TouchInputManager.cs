using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Common.Input
{
    public class TouchInputManager
    {
        #region Data
        public struct InputData
        {
            public UnityEngine.InputSystem.TouchPhase Phase;
            public Vector2 StartPosition;
            public Vector2 EndPosition;
            public Vector2 Direction;
            public float Distance;
        }
        #endregion
        
        private InputData _inputData; 
        private bool _isEnable;

        public event Action<InputData> InputTouchEvent;

        public TouchInputManager()
        {
            _inputData = new InputData();
        }


        public void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();
            _isEnable = true;
        }
        
        
        public void OnDisable()
        {
            EnhancedTouchSupport.Disable();
            TouchSimulation.Disable();
            _isEnable = false;
        }
        
        
        public void OnUpdate()
        {
            if (!_isEnable) return;
            
            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    OnBegan(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    OnStationary(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    OnMoved(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                {
                    OnEnded(touch);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    Debug.Log("TouchPhase.Canceled");
                }
                else
                {
                    Debug.Log("TouchPhase.None");
                }
            }
        }
        

        private void OnBegan(Touch touch)
        {
            _inputData.Phase = touch.phase;
            _inputData.StartPosition = touch.startScreenPosition;
            _inputData.EndPosition = _inputData.StartPosition;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;
            
            UpdateInputData();
        }
        

        private void OnMoved(Touch touch)
        {
            _inputData.Phase = touch.phase;
            _inputData.EndPosition = touch.screenPosition;
            
            var result = _inputData.EndPosition - _inputData.StartPosition;
            _inputData.Distance = result.magnitude;
            _inputData.Direction = (result == Vector2.zero) ? Vector2.zero: result / _inputData.Distance;
            
            UpdateInputData();
        }


        private void OnEnded(Touch touch)
        {
            _inputData.Phase = touch.phase;
            _inputData.EndPosition = touch.screenPosition;
            _inputData.Direction = Vector2.zero;
            _inputData.Distance = 0f;
            
            UpdateInputData();
        }


        private void OnStationary(Touch touch)
        {
            _inputData.Phase = touch.phase;
            _inputData.EndPosition = touch.screenPosition;
            
            var result = _inputData.EndPosition - _inputData.StartPosition;
            _inputData.Distance = result.magnitude;
            _inputData.Direction = (result == Vector2.zero) ? Vector2.zero: result / _inputData.Distance;
            
            UpdateInputData();
        }


        private void UpdateInputData() 
        {            
            InputTouchEvent?.Invoke(_inputData);
        }
    }
}