using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Fire Status", menuName = "Spell System/Effect/Status Effect/Fire Status")]
    public class FireStatus : StatusEffect
    {
        [SerializeField] private int burnTurns;
        [SerializeField] private int burnDamage;

        protected override void ApplyStatusEffects(Unit target)
        {
            target.AddEffet(this);
        }

        protected override void StatusExecute(Unit target)
        {
            
        }
    }
}