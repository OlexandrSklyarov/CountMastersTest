
namespace Source.Gameplay.Player.FSM.States
{
    public class LevelFailureState : BasePlayerState
    {
        public LevelFailureState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            _agent.Failure();
        }


        public override void OnStop()
        {
            throw new System.NotImplementedException();
        }
    }
}