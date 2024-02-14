using Elementals;
using Units;
using UnityEngine;

namespace Spells
{
    public abstract class SpellEffect : ScriptableObject
    {
        [SerializeField] protected int duration;
        [SerializeField] protected ElementalType[] elementalTypes;

        public int Duration { get => duration; set => duration = value; }
        public ElementalType[] ElementalTypes { get => elementalTypes; set => elementalTypes = value; }

        public abstract void ApplyEffect(Unit target);
    }
}