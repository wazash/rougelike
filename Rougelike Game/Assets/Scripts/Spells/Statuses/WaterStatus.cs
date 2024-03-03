using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Water Status", menuName = "Spell System/Effect/Status Effect/Water Status")]
    public class WaterStatus : StatusEffect
    {
        protected override void ApplyStatusEffects(Unit target)
        {
            base.ApplyStatusEffects(target);

            if (!canBeApplied)
                return;

            target.AddStatus(this);
        }

        public override void StatusExecute(Unit target)
        {

        }

        public override string GetDescription()
        {
            return $"apply wetness status.";
        }
    }
}