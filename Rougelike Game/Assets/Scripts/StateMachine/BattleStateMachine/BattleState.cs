using Cards;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    public abstract class BattleState : State<GameLoopStateMachine>
    {
        [InlineEditor]
        [SerializeField] protected EnemiesPack enemiesPack;

        protected StartingDeckConfig deckConfiguration;
        protected Card cardPrefab;
        protected Transform gameplayDeckTransform;
        protected List<Transform> playerPositions;
        protected List<Transform> enemiesPositions;

        protected BattleInitRoutines battleInitRoutines;
        protected BattleInitData battleInitData;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            battleInitData = new(deckConfiguration, cardPrefab, gameplayDeckTransform, playerPositions, enemiesPositions);
            battleInitRoutines ??= new BattleInitRoutines(parent, battleInitData, enemiesPack);

            GetRequiredData();

            // Afterthe data is set, the state machine will enter to the initialization state
            machine.SetState(typeof(InitializeBattleState));
        }

        private void GetRequiredData()
        {
            deckConfiguration = machine.GameManager.DeckConfiguration;
            cardPrefab = machine.GameManager.CardPrefab;
            gameplayDeckTransform = machine.GameManager.DeckPositions.GameplayDeckTransform;

            playerPositions = machine.GameManager.UnitsGroundManager.GetGroundPositions(machine.GameManager.UnitsGroundManager.PlayerGround);
            enemiesPositions = machine.GameManager.UnitsGroundManager.GetGroundPositions(machine.GameManager.UnitsGroundManager.EnemiesGround, true);
        }
    }

}
