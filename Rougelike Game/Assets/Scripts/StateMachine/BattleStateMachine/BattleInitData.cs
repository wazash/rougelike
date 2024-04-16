using Cards;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    public struct BattleInitData
    {
        public Card cardPrefab;
        public Transform gameplayDeckTransform;
        public List<Transform> playerPositions;
        public List<Transform> enemiesPositions;

        public BattleInitData(Card cardPrefab,
                              Transform gameplayDeckTransform,
                              List<Transform> playerPositions,
                              List<Transform> enemiesPositions)
        {
            this.cardPrefab = cardPrefab;
            this.gameplayDeckTransform = gameplayDeckTransform;
            this.playerPositions = playerPositions;
            this.enemiesPositions = enemiesPositions;
        }
    }
}
