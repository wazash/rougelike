using Managers;
using System.Collections;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Units/PlayerData")]
    public class PlayerData : UnitData
    {
        [SerializeField] private Player playerPrefab;

        public Player PlayerPrefab => playerPrefab;

        public IEnumerator SpawnPlayer(Transform spawnPosition, Transform spawnParent)
        {
            var player = Instantiate(playerPrefab, spawnPosition.position, Quaternion.identity, spawnParent);
            player.SetPlayerData(this);
            yield return new WaitForEndOfFrame();
            InitializeData(player);
            GameManager.Instance.UnitsManager.RegisterPlayer(player);
        }
    }
}
