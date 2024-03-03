using System.Linq;
using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Fire Status", menuName = "Spell System/Effect/Status Effect/Fire Status")]
    public class FireStatus : StatusEffect
    {
        [SerializeField] private int burnTurns;
        [SerializeField] private int burnDamage;

        private int turns;
        private int damage;

        protected override void ApplyStatusEffects(Unit target)
        {
            base.ApplyStatusEffects(target);

            if (!canBeApplied)
                return;

            if (HasActiveEffect(target))
            {
                StatusEffect existingStatus = target.ActiveStatuses.Find(status => status.GetType() == GetType());
                if (existingStatus is FireStatus existingFireStatus)
                {
                    existingFireStatus.turns = Mathf.Max(existingFireStatus.turns, burnTurns);
                    existingFireStatus.damage = Mathf.Max(existingFireStatus.damage, burnDamage);
                }
            }
            else
            {
                target.AddStatus(this);

                turns = burnTurns;
                damage = burnDamage;
            }
        }

        public override void StatusExecute(Unit target)
        {
            if (target == null || target.HealthComponent == null)
                return;

            turns--;

            target.HealthComponent.TakeShieldedDamage(damage);
            Debug.Log($"Executed status: Fire Status | damage : {damage}, turns left: {turns}");

            if (turns <= 0)
            {
                target.AddStatusToRemoveList(this);
            }
        }

        public override string GetDescription()
        {
            string statusTypesDescription = string.Join("/", elementalTypes.Select(type => type.TypeName));
            return $"apply {burnDamage} burning for {burnTurns} turns";
        }
    }
}