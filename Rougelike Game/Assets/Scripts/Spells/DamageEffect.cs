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
            if (!target.TryGetComponent(out IDamagable damagable))
            {
                Debug.LogWarning($"{target.name} is missing IDamagable component!");
                return;
            }

            float damageMultiplier = CalculateDamageMultiplier(target);
            int newDamage = CalculateNewDamage(damageMultiplier);

            Shield targetShield = target.ShieldComponent;

            if(targetShield != null ) 
            {
                int shieldDamage = Mathf.Min(target.ShieldComponent.CurrentShield, newDamage);

                newDamage -= shieldDamage;

                target.ShieldComponent.RemoveShield(shieldDamage);
                Debug.Log($"Dealt {shieldDamage} damage to {target.name}'s shield.");
            }

            if (newDamage > 0)
            {
                DealDamage(damagable, newDamage);
                Debug.Log($"Apply {newDamage} dmg (x:{damageMultiplier}) to {target.name}.");
            }
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

        private void DealDamage(IDamagable target, int newDamage) => target.TakeDamage(newDamage);
    }
}