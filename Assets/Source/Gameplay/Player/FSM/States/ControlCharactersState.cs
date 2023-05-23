using System;
using Common.Input;
using UnityEngine;
using Phase = UnityEngine.InputSystem.TouchPhase;

namespace Source.Gameplay.Player.FSM.States
{
    public class ControlCharactersState : BasePlayerState
    {
        private Camera _camera;
        private Vector3 _startPosition;
        private Vector3 _controllerPosition;

        public ControlCharactersState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
            _camera = Camera.main;
        }


        public override void OnStart()
        {
            _agent.Input.InputTouchEvent += OnInputHandler;

            
        }
        

        public override void OnStop()
        {
            _agent.Input.InputTouchEvent -= OnInputHandler;
        }


        private void OnInputHandler(TouchInputManager.InputData data)
        {            
            Debug.Log($"data.Phase {data.Phase}");
            switch(data.Phase)
            {
                case Phase.Began:

                    SetStartPosition(data);

                    break;

                case Phase.Stationary:
                case Phase.Moved:

                    Move(data); 
                     
                    break;

                case Phase.Ended:

                    StopMove();

                    break;
            }           
        }


        private void StopMove()
        {
            _agent.StickmanController.Stop();
        }


        private void SetStartPosition(TouchInputManager.InputData data)
        {
            var plane = new Plane(Vector3.up, 0f);
            _startPosition = Vector3.zero;
            _controllerPosition = _agent.StickmanController.Transform.position;
            TryGetMousePosition(plane, data.EndPosition, ref _startPosition);
        }       


        private void Move(TouchInputManager.InputData data)
        {
            var plane = new Plane(Vector3.up, 0f);
            var endPosition = Vector3.zero;

            if (!TryGetMousePosition(plane, data.EndPosition, ref endPosition)) return;

            var offset = endPosition - _startPosition;
            var curPos = _agent.StickmanController.Transform.position;
            var newPos = _controllerPosition + offset;
            var vertical = curPos + _agent.StickmanController.Transform.forward;

            var x = Mathf.Lerp(curPos.x, newPos.x, Time.deltaTime * _agent.Config.Movement.HorizontalSpeed);
            var z = Mathf.Lerp(curPos.z, vertical.z, Time.deltaTime * _agent.Config.Movement.VerticalSpeed);
            
            _agent.StickmanController.Move(new Vector3(x, curPos.y, z));
        }


        private bool TryGetMousePosition(Plane plane, Vector3 pointer, ref Vector3 position)
        {
            var ray = _camera.ScreenPointToRay(pointer);

            if (plane.Raycast(ray, out var dist))
            {
                position = ray.GetPoint(dist + 1f);
                return true;
            }

            return false;
        }        
    }
}