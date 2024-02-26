using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "SpellFactory", menuName = "Factories/SpellFactory")]
    public class SpellFactory : ScriptableObject
    {
        [SerializeField] private List<Spell> allSpells;

        public Spell GetSpellByName(string spellName) => allSpells.FirstOrDefault(spell => spell.SpellName == spellName);

        public Spell GetRandomSpell()
        {
            if(allSpells == null || allSpells.Count == 0)
            {
                Debug.LogWarning("Spellactory: No spells available.");
                return null;
            }

            int randomIndex = Random.Range(0, allSpells.Count);
            return allSpells[randomIndex];
        }
    }
}


