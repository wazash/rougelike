using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Water Status", menuName = "Spell System/Effect/Status Effect/Water Status")]
    public class WaterStatus : StatusEffect
    {
        protected override void ApplyStatusEffects(Unit target)
        {
            target.AddEffet(this);
        }

        protected override void StatusExecute(Unit target)
        {

        }
    }
}