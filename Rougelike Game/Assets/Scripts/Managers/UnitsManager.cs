using Sirenix.OdinInspector;
using System;
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
    }
}
