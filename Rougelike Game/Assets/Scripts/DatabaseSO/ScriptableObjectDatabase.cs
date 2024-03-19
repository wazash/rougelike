using Elementals;
using Spells;
using Units;
using UnityEngine;

namespace Database
{
    [CreateAssetMenu(fileName = "ScriptableObjectDatabase", menuName = "Database/ScriptableObjectDatabase")]
    public class ScriptableObjectDatabase : ScriptableObject
    {
        public StatusEffect[] StatusEffects;
        public ElementalType[] ElementalTypes;
        public PlayerData[] PlayerDatas;

        private static ScriptableObjectDatabase instance;

        public static ScriptableObjectDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<ScriptableObjectDatabase>("ScriptableObjectDatabase");
                }
                return instance;
            }
        }


        public StatusEffect GetStatusEffectByName(string name)
        {
            foreach (StatusEffect statusEffect in StatusEffects)
            {
                if (statusEffect.name == name)
                {
                    return statusEffect;
                }
            }
            return null;
        }

        public ElementalType GetElementalTypeByName(string name)
        {
            foreach (ElementalType elementalType in ElementalTypes)
            {
                if (elementalType.TypeName == name)
                {
                    return elementalType;
                }
            }
            return null;
        }

        public PlayerData GetPlayerDataByName(string name)
        {
            foreach (PlayerData playerData in PlayerDatas)
            {
                if (playerData.name == name)
                {
                    return playerData;
                }
            }
            return null;
        }
    }
}