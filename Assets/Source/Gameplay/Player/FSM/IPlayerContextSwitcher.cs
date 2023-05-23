
namespace Source.Gameplay.Player.FSM
{
    public interface IPlayerContextSwitcher
    {
        void SwitchState<T>() where T : BasePlayerState;
    }
}