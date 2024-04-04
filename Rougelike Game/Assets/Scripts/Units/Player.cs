using DG.Tweening;
using Elementals;
using Managers;
using NewSaveSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
    public struct PlayerSaveData
    {
        [Title("Health Data")]
        public int MaxHealth;
        public int CurrentHealth;
        public int CurrentShield;
        public float ShieldLossPercentage;

        [Title("Unit Data")]
        public string PlayerDataName;
        public List<string> ActiveStatusesNames;
        public string[] ElementalTypesNames;

        public PlayerSaveData(Player player)
        {
            MaxHealth = player.HealthComponent.MaxHealt;
            CurrentHealth = player.HealthComponent.CurrentHealth;
            CurrentShield = player.HealthComponent.CurrentShield;
            ShieldLossPercentage = player.HealthComponent.ShieldLossPercentage;
            PlayerDataName = player.PlayerData.name;
            ActiveStatusesNames = player.ActiveStatuses.Select(se => se.name).ToList();
            ElementalTypesNames = player.ElementalTypes.Select(et => et.TypeName).ToArray();
        }
    }

    public class Player : Unit, ISaveable
    {
        [InlineEditor]
        [SerializeField] private PlayerData playerData;

        private Tween moveToPositionTween;

        public PlayerData PlayerData => playerData;

        public Tween MoveToPositionTween { get => moveToPositionTween; set => moveToPositionTween = value; }

        protected override void Start()
        {
            base.Start();

            healthComponent.OnDeath += OnDeath;
        }

        internal void SetPlayerData(PlayerData playerData) => this.playerData = playerData;

        private void OnDeath()
        {
            // Player death logic
        }

        #region Saving
        public Type GetDataType() => typeof(PlayerSaveData);

        public string GetSaveID() => "Player";

        public object Save()
        {
            if (playerData == null)
            {
                Debug.LogError("Player data is null");
                return null;
            }

            return new PlayerSaveData(this);
        }

        public void Load(object saveData)
        {
            PlayerSaveData playerSaveData = (PlayerSaveData)saveData;

            playerData = GameManager.Instance.Database.GetPlayerDataByName(playerSaveData.PlayerDataName);

            healthComponent.SetMaxHealth(playerSaveData.MaxHealth);
            healthComponent.SetCurrentHealth(playerSaveData.CurrentHealth);
            healthComponent.SetCurrentShield(playerSaveData.CurrentShield);
            healthComponent.SetShieldLossPercentage(playerSaveData.ShieldLossPercentage);

            activeStatuses.Clear();
            foreach (var statusName in playerSaveData.ActiveStatusesNames)
            {
                var statusEffect = GameManager.Instance.Database.GetStatusEffectByName(statusName);
                if (statusEffect != null)
                {
                    activeStatuses.Add(statusEffect);
                }
                else
                {
                    Debug.LogWarning($"Status effect with name {statusName} not found in database");
                }
            }

            elementalTypes = new ElementalType[playerSaveData.ElementalTypesNames.Length];
            for (int i = 0; i < playerSaveData.ElementalTypesNames.Length; i++)
            {
                var elementalType = GameManager.Instance.Database.GetElementalTypeByName(playerSaveData.ElementalTypesNames[i]);
                if (elementalType != null)
                {
                    elementalTypes[i] = elementalType;
                }
                else
                {
                    Debug.LogWarning($"Elemental type with name {playerSaveData.ElementalTypesNames[i]} not found in database");
                }
            }
        }
        #endregion

        public void SetMoveToPositionTween(Transform groundPosition)
        {
            moveToPositionTween = transform.DOMove(groundPosition.position, .5f).SetEase(Ease.InOutSine);
            moveToPositionTween.Pause();
        }
    }
}
