
namespace Source.Gameplay.Player.FSM.States
{
    public class LevelCompletedState : BasePlayerState
    {
        public LevelCompletedState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            _agent.Success();
        }


        public override void OnStop()
        {
        }
    }
}