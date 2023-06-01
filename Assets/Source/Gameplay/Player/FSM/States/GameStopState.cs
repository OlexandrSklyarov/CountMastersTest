
namespace Source.Gameplay.Player.FSM.States
{
    public class GameStopState : BasePlayerState
    {
        public GameStopState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {            
        }


        public override void OnStop()
        {
        }
    }
}