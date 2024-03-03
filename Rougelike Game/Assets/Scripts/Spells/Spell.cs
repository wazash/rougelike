using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "Spell System/Spell")]
    public class Spell : ScriptableObject
    {
        [SerializeField] private string spellName;
        [SerializeField, TextArea(2,3)] private string spellDescription;

        [PropertySpace]

        [InlineEditor]
        [SerializeField] private SpellEffect[] spellEffects;

        public string SpellName { get => spellName; }
        public string SpellDescription { get => spellDescription; }
        public SpellEffect[] SpellEffects { get => spellEffects; }

        public void UpdateDescription()
        {
            var descriptions = new List<string>();

            foreach (var effect in spellEffects)
            {
                descriptions.Add(effect.GetDescription());
            }

            spellDescription = string.Join(" and ", descriptions);
        }


        private void OnValidate()
        {
            UpdateDescription();
        }
    }
}


