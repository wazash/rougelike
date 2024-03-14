using Elementals;
using Managers;
using System.Collections;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Units/EnemyData")]
    public class EnemyData : UnitData
    {
        [SerializeField] private Enemy enemyPrefab;

        public IEnumerator SpawnEnemy(Transform spawnPosition, Transform spawnParent)
        {
            var enemy = Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity, spawnParent);
            enemy.SetEnemyData(this);
            yield return new WaitForEndOfFrame();
            InitializeData(enemy);
            GameManager.Instance.UnitsManager.RegisterEnemy(enemy);
        }
    }
}
