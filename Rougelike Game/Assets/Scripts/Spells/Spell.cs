using Sirenix.OdinInspector;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "Spell System/Spell")]
    public class Spell : ScriptableObject
    {
        [SerializeField] private string spellName;
        [SerializeField] private string spellDescription;

        [PropertySpace]

        [SerializeField] private SpellEffect[] spellEffects;

        public string SpellName { get => spellName; set => spellName = value; }
        public string SpellDescription { get => spellDescription; set => spellDescription = value; }
        public SpellEffect[] SpellEffects { get => spellEffects; set => spellEffects = value; }
    }
}
