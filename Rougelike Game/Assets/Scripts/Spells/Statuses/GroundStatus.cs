using Sirenix.OdinInspector;
using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Ground Status", menuName = "Spell System/Effect/Status Effect/Ground Status")]
    public class GroundStatus : StatusEffect
    {
        [PropertySpace]

        [SerializeField] private int shieldStrength;

        protected override void ApplyStatusEffects(Unit target)
        {
            base.ApplyStatusEffects(target);

            if (!canBeApplied)
                return;

            target.HealthComponent.AddShield(shieldStrength);
            target.AddStatus(this);
        }

        public override void StatusExecute(Unit target)
        {
            if (target.HealthComponent == null)
            {
                Debug.Log($"{target.name} is missing health component.");
                return;
            }

            float percentage = target.HealthComponent.ShieldLossPercentage;
            target.HealthComponent.RemoveShieldPercentage(percentage);
        }
    }
}