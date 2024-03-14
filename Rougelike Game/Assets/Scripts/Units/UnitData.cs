using Elementals;
using UnityEngine;

namespace Units
{
    public abstract class UnitData : ScriptableObject
    {
        [SerializeField] protected Sprite unitSprite;

        [SerializeField] protected string unitName;
        [SerializeField] protected string unitNameKey;
        [SerializeField] private string unitDescription;
        [SerializeField] protected string unitDescriptionKey;
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int startingShield;
        [SerializeField] protected ElementalType[] elementalTypes;

        public Sprite UnitSprite => unitSprite;
        public string UnitName => unitName;
        public string UnitNameKey => unitNameKey;
        public string UnitDescription => unitDescription;
        public string UnitDescriptionKey => unitDescriptionKey;

        public void InitializeData(Unit unit)
        {
            if(unit == null)
            {
                Debug.LogError("Unit is null");
                return;
            }
            if(unit.HealthComponent == null)
            {
                Debug.LogError("Unit health component is null");
                return;
            }
            if(unit.UnitVisuals == null)
            {
                Debug.LogError("Unit visuals are null");
                return;
            }

            unit.HealthComponent.SetMaxHealth(maxHealth);
            unit.HealthComponent.SetStartingShield(startingShield);
            unit.UnitVisuals.SetSprite(unitSprite);

            unit.SetElementalTypes(elementalTypes);
        }
    }
}
