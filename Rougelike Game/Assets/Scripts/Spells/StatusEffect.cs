using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Spells
{
    public abstract class StatusEffect : SpellEffect
    {
        [SerializeField] protected bool triggerAtTurnStart;
        [SerializeField] protected bool triggerAtTurnEnd;

        protected bool canBeApplied;

        public bool TriggerAtTurnStart { get => triggerAtTurnStart; set => triggerAtTurnStart = value; }
        public bool TriggerAtTurnEnd { get => triggerAtTurnEnd; set => triggerAtTurnEnd = value; }

        public override void ApplyEffect(Unit target) => ApplyStatusEffects(target);

        protected virtual void ApplyStatusEffects(Unit target)
        {
            if (target.HealthComponent == null)
            {
                Debug.Log($"{target.name} is missing health component.");
                canBeApplied = false;
                return;
            }

            if (target.IsImmune(this))
            {
                canBeApplied = false;
                return;
            }

            List<StatusEffect> conflictingStatuses = GetConflictingStatuses(target);

            foreach (StatusEffect status in conflictingStatuses)
            {
                target.AddStatusToRemoveList(status);
            }

            if (conflictingStatuses.Count > 0)
            {
                target.UpdateStatuses();
            }

            canBeApplied = true;
        }

        protected List<StatusEffect> GetConflictingStatuses(Unit target)
        {
            return target.ActiveStatuses
                    .Where(targetActiveStatus => targetActiveStatus.ElementalTypes
                        .Any(targetActiveStatusType => this.ElementalTypes
                            .Any(incomingStatusType => incomingStatusType.Strengths.Contains(targetActiveStatusType))))
                    .ToList();
        }

        public abstract void StatusExecute(Unit target);

        protected bool HasActiveEffect(Unit target)
        {
            return target.ActiveStatuses.Any(effect => effect.GetType() == GetType());
        }
    }
}