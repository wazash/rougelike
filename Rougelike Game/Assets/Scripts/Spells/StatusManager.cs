using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Spells
{
    public class StatusManager : MonoBehaviour
    {
        [SerializeField] private List<Unit> unitList = new();

        // debug in buttons
        public void ExeStart()
        {
            ExecuteStatusEffectsStart(unitList);
        }

        // debug in buttons
        public void ExeEnd()
        {
            ExecuteStatusEffectsEnd(unitList);
        }

        public void ExecuteStatusEffectsStart(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                foreach (StatusEffect statusEffect in unit.ActiveStatuses)
                {
                    if(statusEffect.TriggerAtTurnStart)
                    {
                        statusEffect.StatusExecute(unit);
                    }
                }

                unit.UpdateStatuses();
            }
        }
        public void ExecuteStatusEffectsEnd(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                foreach (StatusEffect statusEffect in unit.ActiveStatuses)
                {
                    if (statusEffect.TriggerAtTurnEnd)
                    {
                        statusEffect.StatusExecute(unit);
                    }
                }

                unit.UpdateStatuses();
            }
        }
    }
}