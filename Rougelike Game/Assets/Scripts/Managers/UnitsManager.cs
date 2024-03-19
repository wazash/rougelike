using Healths;
using NewSaveSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Managers
{
    public class UnitsManager : MonoBehaviour
    {
        [ShowInInspector]
        private readonly List<Enemy> enemies = new();
        [ShowInInspector]
        private Player player;

        public List<Enemy> Enemies => enemies;
        public Player Player => player;

        public event Action OnEnemiesCleared;

        public void RegisterEnemy(Enemy enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
            else
            {
                Debug.LogWarning($"Tried to register {enemy.name} but it was already in the list of enemies.");
                return;
            }
        }

        public void UnregisterEnemy(Enemy enemy)
        {
            if(enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
            else
            {
                Debug.LogWarning($"Tried to unregister {enemy.name} but it was not found in the list of enemies.");
                return;
            }

            if (enemies.Count == 0)
            {
                OnEnemiesCleared?.Invoke();
            }
        }

        public void RegisterPlayer(Player player)
        {
            if (this.player == null)
            {
                this.player = player;
            }
            else
            {
                Debug.LogWarning("Tried to register the player but it was already registered.");
                return;
            }
        }

        public void CreateEmptyPlayer()
        {

            var spawnPosition = GameManager.Instance.UnitsGroundManager.PlayerSpawnPoint;
            var spawnParent = GameManager.Instance.UnitsGroundManager.PlayerGround;

            player = Instantiate(GameManager.Instance.PlayerPrefab, spawnPosition.position, Quaternion.identity, spawnParent);

            SaveManager.RegisterSaveable(player);
        }
    }
}
