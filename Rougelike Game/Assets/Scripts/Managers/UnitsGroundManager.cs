using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UnitsGroundManager : MonoBehaviour
    {
        [SerializeField] private Transform playerGround;
        [SerializeField] private Transform playerSpawnPoint;

        [PropertySpace]

        [SerializeField] private Transform enemiesGround;
        [SerializeField] private Transform enemySpawningPoint;

        public Transform PlayerGround => playerGround;
        public Transform PlayerSpawnPoint => playerSpawnPoint;

        public Transform EnemiesGround => enemiesGround;
        public Transform EnemySpawningPoint => enemySpawningPoint;

        public List<Transform> GetGroundPositions(Transform ground, bool reverse = false)
        {
            List<Transform> groundPositions = new();

            for (int i = 0; i < ground.childCount; i++)
            {
                groundPositions.Add(ground.GetChild(i));
            }

            if (reverse)
            {
                groundPositions.Reverse();
            }

            Debug.Log($"Enemies ground positions count: {groundPositions.Count}");

            return groundPositions;
        }
    }
}
