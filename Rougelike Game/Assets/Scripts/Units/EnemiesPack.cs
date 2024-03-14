using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "EnemiesPack", menuName = "Units/EnemiesPack")]
    public class EnemiesPack : ScriptableObject
    {
        [SerializeField] private List<EnemyData> enemiesList = new();

        public List<EnemyData> UnitList => enemiesList;

    }
}
