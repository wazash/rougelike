using Elementals;
using Healths;
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
        protected List<StatusEffect> statusesToRemove = new();
        [SerializeField] protected ElementalType[] elementalTypes;

        protected Health healthComponent;

        public List<StatusEffect> ActiveStatuses { get => activeStatuses; }
        public ElementalType[] ElementalTypes { get => elementalTypes; set => elementalTypes = value; }
        public Health HealthComponent { get => healthComponent; set => healthComponent = value; }

        private void Start()
        {
            healthComponent = GetComponent<Health>();
        }

        /// <summary>
        /// Adds a status effect to the unit if it's not immune
        /// </summary>
        /// <param name="effect"></param>
        public void AddStatus(StatusEffect effect)
        {
            if (IsImmune(effect))
            {
                return;
            }

            activeStatuses.Add(effect);
        }

        /// <summary>
        /// Add a status to the list of statuses to remove
        /// </summary>
        /// <param name="effect"></param>
        public void AddStatusToRemoveList(StatusEffect effect)
        {
            if (activeStatuses.Contains(effect))
            {
                statusesToRemove.Add(effect);
            }
        }

        /// <summary>
        /// Updates the active statuses list by removing statuses marked for removal
        /// </summary>
        public void UpdateStatuses()
        {
            foreach (var statusToRemove in statusesToRemove)
            {
                activeStatuses.Remove(statusToRemove);
            }

            statusesToRemove.Clear();
        }

        public bool IsImmune(StatusEffect incomingEffect)
        {
            // Check id any of the unit's elemental types provide immunity to the incoming status effect
            if (ElementalTypes.Any(unitType => incomingEffect.ElementalTypes.Any(effectType => effectType.Weaknesses.Contains(unitType))))
            {
                // Capture the unitType in a separate variable
                ElementalType immuneType = ElementalTypes.First(unitType => incomingEffect.ElementalTypes.Any(effectType => effectType.Weaknesses.Contains(unitType)));

                Debug.Log($"{name} is immune to the {incomingEffect.GetType().Name} effect due to its unit elemental type ( {immuneType.TypeName} ).");
                return true;
            }

            // Check if the unit has any active status effects that provide immunity to the incoming status effect
            if (activeStatuses.Any(activeStatus =>
                incomingEffect.ElementalTypes.Any(effectType =>
                    activeStatus.ElementalTypes.Any(statusType =>
                        effectType.Weaknesses.Contains(statusType)))))
            {
                Debug.Log($"{name} is immune to the {incomingEffect.GetType().Name} effect due to an active status effect.");
                return true;
            }

            return false;
        }

        public List<ElementalType> GetActiveStatusTypes() => activeStatuses.SelectMany(status => status.ElementalTypes).Distinct().ToList();
    }
}
