using System;
using Source.Data;
using Source.Gameplay.Characters;
using Source.Gameplay.Player.FSM;
using Source.Gameplay.Player.FSM.States;
using System.Collections.Generic;
using System.Linq;
using Common.Input;

namespace Source.Gameplay.Player
{
    public sealed class PlayerController : IPlayer, IPlayerContextSwitcher
    {
        PlayerData IPlayer.Config => _config;
        IStickmanController IPlayer.StickmanController => _stickmanController;
        TouchInputManager IPlayer.Input => _input;

        private readonly PlayerData _config;
        private readonly IStickmanController _stickmanController;
        private readonly TouchInputManager _input;
        private readonly Dictionary<Type,BasePlayerState> _allStates;
        private BasePlayerState _currentState;
        private bool _isActive;

        public event Action SuccessEvent;
        public event Action FailureEvent;
        

        public PlayerController(PlayerData config, IStickmanController stickmanController, TouchInputManager input)
        {
            _config = config;
            _stickmanController = stickmanController;
            _input = input;

            _allStates = new Dictionary<Type, BasePlayerState>()
            {
                {typeof(WaitTapToPlayState), new WaitTapToPlayState(this, this)},
                {typeof(ControlCharactersState), new ControlCharactersState(this, this)},
                {typeof(LevelCompletedState), new LevelCompletedState(this, this)},
                {typeof(LevelFailureState), new LevelFailureState(this, this)},
            };

            _currentState = _allStates.First().Value;
        }
        

        public void Enable()
        {
            if (_isActive) return;

            _isActive = true;
            _currentState?.OnStart();
        }


        public void Disable()
        {
            if (!_isActive) return;

            _isActive = false;
            _currentState?.OnStop();
        }


        public void OnUpdate()
        {
            if (!_isActive) return;
            
        }


        public void SwitchState<T>() where T : BasePlayerState
        {
            var state = _allStates[typeof(T)];

            _currentState?.OnStop();
            _currentState = state;
            _currentState?.OnStart();
        }


        void IPlayer.Success() => SuccessEvent?.Invoke();


        void IPlayer.Failure() => FailureEvent?.Invoke();
    }
}