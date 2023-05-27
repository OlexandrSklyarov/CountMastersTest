using Source.Gameplay.Characters;
using Source.Gameplay.Environment;
using UnityEngine;

public class FinishLine : BaseStickmanTrigger
{
    protected override void Interact(Collider other)
    {
        if (other.TryGetComponent(out IFormationGroup group))
        {
            group.FinishFormation();
        }
    }
}
