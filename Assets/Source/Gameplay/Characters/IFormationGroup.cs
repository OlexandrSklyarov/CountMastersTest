
namespace Source.Gameplay.Characters
{
    public interface IFormationGroup
    {
        int Count { get; }

        void FinishFormation();
        void Refresh();
    }
}