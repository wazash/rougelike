using Units;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "New Status Effect", menuName = "Spell System/Effect/Status Effect")]
    public class StatusEffect : SpellEffect
    {
        public override void ApplyEffect(Unit target)
        {
            string message = string.Empty;

            foreach(var status in ElementalTypes)
            {
                target.AddEffet(this);

                message += $"{status.TypeName} ";
            }

            Debug.Log($"Apply {message} status to {target.name}");
        }
    }
}