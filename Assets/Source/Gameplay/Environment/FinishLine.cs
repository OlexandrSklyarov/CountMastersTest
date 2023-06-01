using System;
using Source.Gameplay.Characters;
using Source.Gameplay.Environment;
using UnityEngine;

public class FinishLine : BaseStickmanTrigger
{
    public event Action<int> FinishTriggerEvent;

    protected override void Interact(Collider other)
    {
        if (other.TryGetComponent(out IFormationGroup group))
        {
            group.FinishFormation();
            FinishTriggerEvent?.Invoke(group.Count);
        }
    }
}
