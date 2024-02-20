using System;
using UnityEngine;

namespace Healths
{
    public class Health : MonoBehaviour, IDamagable, IHealable
    {
        [SerializeField] private int maxHealth;

        private int currentHealth;

        public int MaxHealt { get => maxHealth; set => maxHealth = value; }
        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        public event Action<int> OnHealthChanged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;
            OnHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(int healAmount)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} died!");
            Destroy(gameObject);
        }
    }
}
