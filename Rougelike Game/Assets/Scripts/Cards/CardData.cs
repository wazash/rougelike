using Spells;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "New Card Data", menuName = "Cards/Card Data")]
    public class CardData : ScriptableObject
    {
        [SerializeField] private Spell spell;

        public Spell Spell { get => spell; set => spell = value; }
    }
}
