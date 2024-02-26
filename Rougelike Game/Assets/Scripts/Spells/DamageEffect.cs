using Healths;
using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Damage Effect", menuName = "Spell System/Effect/Damage Effect")]
    public class DamageEffect : SpellEffect
    {
        [SerializeField] private int baseDamage;

        private readonly float minMultiplier = 0.0f;
        private readonly float maxMultiplier = 2.0f;

        public int BaseDamage { get => baseDamage; set => baseDamage = value; }

        public override void ApplyEffect(Unit target)
        {
            if (!target.TryGetComponent(out Health healthComponent))
            {
                Debug.LogWarning($"{target.name} is missing Health component!");
                return;
            }

            float damageMultiplier = CalculateDamageMultiplier(target);
            int newDamage = CalculateNewDamage(damageMultiplier);

            DealDamage(healthComponent, newDamage);
        }

        private float CalculateDamageMultiplier(Unit target)
        {
            float damageMultiplier = 1.0f;

            foreach (var elementalType in elementalTypes)
            {
                foreach (var targetType in target.ElementalTypes)
                {
                    damageMultiplier *= elementalType.GetEffectivnessMultiplier(targetType);
                }
            }

            return Mathf.Clamp(damageMultiplier, minMultiplier, maxMultiplier);
        }

        private int CalculateNewDamage(float damageMultiplier) => Mathf.RoundToInt(baseDamage * damageMultiplier);

        private void DealDamage(Health target, int newDamage) => target.TakeShieldedDamage(newDamage);
    }
}