using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Ground Status", menuName = "Spell System/Effect/Status Effect/Ground Status")]
    public class GroundStatus : StatusEffect
    {
        [SerializeField] private int shieldStrength;

        protected override void ApplyStatusEffects(Unit target)
        {
            if(target.ShieldComponent == null)
            {
                Debug.Log($"{target.name} is missing shield component.");
                return;
            }

            target.ShieldComponent.AddShield(shieldStrength);
            target.AddEffet(this);
        }

        protected override void StatusExecute(Unit target)
        {
            throw new System.NotImplementedException();
        }
    }
}