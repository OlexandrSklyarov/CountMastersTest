using Source.Gameplay.Characters;

namespace Source.Gameplay.Environment
{
    public interface IInteractable
    {
        void Interact(IInteractTarget target);
    }
}