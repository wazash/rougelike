using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Damage Effect", menuName = "Spell System/Effect/Damage Effect")]
    public class DamageEffect : SpellEffect
    {
        [SerializeField] private int baseDamage;

        private float minMultiplier = 0.0f;
        private float maxMultiplier = 2.0f;

        public int BaseDamage { get => baseDamage; set => baseDamage = value; }

        public override void ApplyEffect(Unit target)
        {
            float damageMultiplier = CalculateDamageMultiplier(target);

            int newDamage = CalculateNewDamage(damageMultiplier);

            DealDamage(target, newDamage, damageMultiplier);
        }

        private float CalculateDamageMultiplier(Unit target)
        {
            float damageMultiplier = 1.0f;

            foreach (var elementalType in elementalTypes)
            {
                foreach(var targetType in target.ElementalTypes)
                {
                    damageMultiplier *= elementalType.GetEffectivnessMultiplier(targetType);
                }
            }

            return Mathf.Clamp(damageMultiplier, minMultiplier, maxMultiplier);
        }

        private int CalculateNewDamage(float damageMultiplier)
        {
            return Mathf.RoundToInt(baseDamage * damageMultiplier);
        }

        private void DealDamage(Unit target, int newDamage, float damageMultiplier)
        {
            target.TakeDamage(newDamage);
            Debug.Log($"Apply {newDamage} dmg (x:{damageMultiplier}) to {target.name}.");
        }
    }
}