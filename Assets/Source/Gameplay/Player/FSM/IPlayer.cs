using Common.Input;
using Source.Data;
using Source.Gameplay.Characters;

namespace Source.Gameplay.Player.FSM
{
    public interface IPlayer
    {
        PlayerData Config { get; }
        IStickmanController StickmanController { get; }
        TouchInputManager Input {get;}

        void Success();
        void Failure();
    }
}