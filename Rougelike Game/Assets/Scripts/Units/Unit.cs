using Elementals;
using Healths;
using Sirenix.OdinInspector;
using Spells;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(Health))]
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected List<StatusEffect> activeStatuses = new();
        [SerializeField] protected ElementalType[] elementalTypes;

        protected Health healthComponent;
        private Shield shieldComponent;

        public List<StatusEffect> ActiveStatuses { get => activeStatuses; set => activeStatuses = value; }
        public ElementalType[] ElementalTypes { get => elementalTypes; set => elementalTypes = value; }
        public Health HealthComponent { get => healthComponent; set => healthComponent = value; }
        public Shield ShieldComponent { get => shieldComponent; set => shieldComponent = value; }

        private void Start()
        {
            healthComponent = GetComponent<Health>();
            shieldComponent = new Shield();
        }

        /// <summary>
        /// Adds a status effect to the unit if it's not immune
        /// </summary>
        /// <param name="effect"></param>
        public void AddEffet(StatusEffect effect)
        {
            if (IsImmune(effect))
            {
                return;
            }

            activeStatuses.Add(effect);
        }

        protected bool IsImmune(StatusEffect effect)
        {
            // Check id any of the unit's elemental types provide immunity to the incoming status effect
            if(ElementalTypes.Any(unitType => effect.ElementalTypes.Any(effectType => effectType.Weaknesses.Contains(unitType))))
            {
                Debug.Log($"{name} is immune to the {effect.GetType().Name} status effect due to its unit elemental type.");
                return true;
            }

            // Check if the unit has any active status effects that provide immunity to the incoming status effect
            if (activeStatuses.Any(activeStatus =>
                effect.ElementalTypes.Any(effectType =>
                    activeStatus.ElementalTypes.Any(statusType =>
                        effectType.Weaknesses.Contains(statusType)))))
            {
                Debug.Log($"{name} is immune to the {effect.GetType().Name} status effect due to an active status effect.");
                return true;
            }

            return false;
        }

        public List<ElementalType> GetActiveStatusTypes() => activeStatuses.SelectMany(status => status.ElementalTypes).Distinct().ToList();

        [PropertySpace]
        [ShowInInspector]
        private int ShieldAmount() => shieldComponent?.CurrentShield ?? 0;

        [ShowInInspector]
        private int HealthAmount() => healthComponent.CurrentHealth;
    }
}
