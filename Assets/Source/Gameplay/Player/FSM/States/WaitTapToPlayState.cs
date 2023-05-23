using Common.Input;
using Phase = UnityEngine.InputSystem.TouchPhase;

namespace Source.Gameplay.Player.FSM.States
{
    public class WaitTapToPlayState : BasePlayerState
    {
        public WaitTapToPlayState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
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
            if (data.Phase == Phase.Began) _context.SwitchState<ControlCharactersState>();
        }
    }
}