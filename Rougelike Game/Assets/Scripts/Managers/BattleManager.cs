using Sirenix.OdinInspector;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Battle
{
	public class BattleManager : SerializedMonoBehaviour
	{
		[SerializeField] private GameObject battleScreen;
		[SerializeField] private GameObject winScreen;
		[SerializeField] private GameObject loseScreen;

		public Dictionary<int, EnemiesPack[]> enemiesPerFloor = new(); 

        public GameObject BattleScreen { get => battleScreen; set => battleScreen = value; }
        public GameObject WinScreen { get => winScreen; set => winScreen = value; }
        public GameObject LoseScreen { get => loseScreen; set => loseScreen = value; }

		public EnemiesPack GetRandomEnemiesPack (int floorIndex)
		{
			// If the floor index is not in the dictionary, return null
			if (!enemiesPerFloor.ContainsKey(floorIndex))
			{
                return null;
            }

			// Get the enemies pack array for the given floor index
			EnemiesPack[] enemiesPacks = enemiesPerFloor[floorIndex];

			// Get a random index from the enemies pack array
			int randomIndex = Random.Range(0, enemiesPacks.Length);

			// Return the enemies pack at the random index
			return enemiesPacks[randomIndex];
		}
    } 
}
