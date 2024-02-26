using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Grass Status", menuName = "Spell System/Effect/Status Effect/Grass Status")]
    public class GrassStatus : StatusEffect
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
    }
}