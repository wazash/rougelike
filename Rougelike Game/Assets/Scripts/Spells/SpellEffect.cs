using Elementals;
using Units;
using UnityEngine;

namespace Spells
{
    public abstract class SpellEffect : ScriptableObject
    {
        [SerializeField] protected ElementalType[] elementalTypes;

        public ElementalType[] ElementalTypes { get => elementalTypes; set => elementalTypes = value; }

        public abstract void ApplyEffect(Unit target);
    }
}