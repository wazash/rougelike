using Units;
using UnityEngine;

namespace Spells
{
    public abstract class StatusEffect : SpellEffect
    {
        [SerializeField] protected bool triggerAtTurnStart;
        [SerializeField] protected bool triggerAtTurnEnd;

        protected bool TriggerAtTurnStart { get => triggerAtTurnStart; set => triggerAtTurnStart = value; }
        protected bool TriggerAtTurnEnd { get => triggerAtTurnEnd; set => triggerAtTurnEnd = value; }

        public override void ApplyEffect(Unit target) => ApplyStatusEffects(target);

        protected abstract void ApplyStatusEffects(Unit target);

        protected abstract void StatusExecute(Unit target);

        protected string GetStatusName() => GetType().Name;
    }
}