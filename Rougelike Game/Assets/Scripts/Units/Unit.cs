using Elementals;
using Spells;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private List<SpellEffect> activeEffects = new();
        [SerializeField] private ElementalType[] elementalTypes;

        public int Health { get => health; set => health = value; }
        public List<SpellEffect> ActiveEffects { get => activeEffects; set => activeEffects = value; }
        public ElementalType[] ElementalTypes { get => elementalTypes; set => elementalTypes = value; }

        public void TakeDamage(int damageAmount)
        {
            health -= damageAmount;

            if (health <= 0)
            {
                health = 0;
                KillUnit();
            }
        }

        private void KillUnit()
        {
            Destroy(gameObject);
        }

        public void AddEffet(SpellEffect effect)
        {
            activeEffects.Add(effect);
        }
    }
}
