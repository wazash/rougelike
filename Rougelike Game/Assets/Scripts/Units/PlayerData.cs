using Cards;
using Managers;
using NewSaveSystem;
using System.Collections;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Units/PlayerData")]
    public class PlayerData : UnitData
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private StartingDeckConfig startingDeckConfig;

        public Player PlayerPrefab => playerPrefab;

        public StartingDeckConfig StartingDeckConfig => startingDeckConfig;

        public IEnumerator SpawnPlayer(Transform spawnPosition, Transform spawnParent)
        {
            var player = Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity, spawnParent);
            player.SetPlayerData(this);
            yield return new WaitForEndOfFrame();
            InitializeData(player);
            SaveManager.RegisterSaveable(player);
            GameManager.Instance.UnitsManager.RegisterPlayer(player);
        }
    }
}
