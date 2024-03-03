using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Healths
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField, Range(0.0f, 1.0f)] private float shieldLossPercentage;

        [ShowInInspector]
        private int currentHealth;
        [ShowInInspector]
        private int currentShield;

        public int MaxHealt { get => maxHealth; }
        public int CurrentHealth { get => currentHealth; }
        public int CurrentShield { get => currentShield; }
        public float ShieldLossPercentage { get => shieldLossPercentage; }

        public event Action<int> OnHealthChanged;
        public event Action<int> OnShieldChanged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeHealthDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            OnHealthChanged?.Invoke(currentHealth);

            Debug.Log($"Dealt {damageAmount} damage to {name}'s health.");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeShieldedDamage(int damageAmount)
        {
            int shieldDamage = Mathf.Min(currentShield, damageAmount);
            damageAmount -= shieldDamage;

            currentShield -= shieldDamage;
            OnShieldChanged?.Invoke(currentShield);

            Debug.Log($"Dealt {shieldDamage} damage to {name}'s shield.");

            if (damageAmount > 0)
            {
                TakeHealthDamage(damageAmount);
            }
        }

        public void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        public void AddShield(int shieldAmount)
        {
            currentShield += shieldAmount;
            OnShieldChanged?.Invoke(currentShield);
            Debug.Log($"{gameObject.name} has {CurrentShield} shield");
        }

        public void RemoveShield(int shieldAmount)
        {
            currentShield -= shieldAmount;

            if (currentShield <= 0)
            {
                currentShield = 0;
            }

            Debug.Log($"Removed {shieldAmount} from {gameObject.name}");
            OnShieldChanged?.Invoke(currentShield);
        }

        public void RemoveShieldPercentage(float shieldPercentageRemove)
        {
            int amountToRemove = Mathf.RoundToInt(currentShield * shieldLossPercentage);

            RemoveShield(amountToRemove);
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} died!");
            Destroy(gameObject);
        }
    }
}
