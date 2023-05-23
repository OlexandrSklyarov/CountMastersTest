
namespace Source.Gameplay.Player.FSM
{
    public abstract class BasePlayerState
    {
        protected readonly IPlayerContextSwitcher _context;
        protected readonly IPlayer _agent;

        
        protected BasePlayerState(IPlayerContextSwitcher context, IPlayer agent)
        {
            _context = context;
            _agent = agent;
        }

        public abstract void OnStart();
        public abstract void OnStop();
        public virtual void OnUpdate() {}
        public virtual void OnLateUpdate() {}
        public virtual void OnFixedUpdate() {}
    }
}