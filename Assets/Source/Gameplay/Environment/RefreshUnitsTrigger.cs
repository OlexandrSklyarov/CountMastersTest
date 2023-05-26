using Source.Gameplay.Characters;
using UnityEngine;

namespace Source.Gameplay.Environment
{
    public class RefreshUnitsTrigger : BaseStickmanTrigger
    {
        protected override void Interact(Collider other)
        {
            if (other.TryGetComponent(out IFormationGroup group))
            {
                group.FormationGroup();
            } 
        }
    }
}