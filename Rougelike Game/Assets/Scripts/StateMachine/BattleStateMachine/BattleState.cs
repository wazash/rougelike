using Cards;
using Managers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "BattleState", menuName = "StateMachine/States/Battle")]
    public class BattleState : State<GameLoopStateMachine>
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

        private static bool firstTime = true;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            if (firstTime)
            {
                parent.BattleScreen.SetActive(true);

                GetRequiredData();
                ChooseEnemiesPack(parent.GameManager.MapManager.currentFloor);

                battleInitData = new(deckConfiguration, cardPrefab, gameplayDeckTransform, playerPositions, enemiesPositions);
                battleInitRoutines ??= new BattleInitRoutines(parent, battleInitData, enemiesPack);

                CoroutineRunner.Start(BattleInitializationRoutine());

                firstTime = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void GetRequiredData()
        {
            deckConfiguration = machine.GameManager.DeckConfiguration;
            cardPrefab = machine.GameManager.CardPrefab;
            gameplayDeckTransform = machine.GameManager.DeckPositions.GameplayDeckTransform;

            playerPositions = machine.GameManager.UnitsGroundManager.GetGroundPositions(machine.GameManager.UnitsGroundManager.PlayerGround);
            enemiesPositions = machine.GameManager.UnitsGroundManager.GetGroundPositions(machine.GameManager.UnitsGroundManager.EnemiesGround, true);
        }

        private void ChooseEnemiesPack(int floorIndex)
        {
            enemiesPack = machine.GameManager.BattleManager.GetRandomEnemiesPack(floorIndex);
        }

        private IEnumerator BattleInitializationRoutine()
        {
            yield return CoroutineRunner.Start(battleInitRoutines.PrepareDeckRoutine());

            yield return CoroutineRunner.Start(battleInitRoutines.SetPlayerRoutine());
            yield return CoroutineRunner.Start(battleInitRoutines.SetEnemiesRoutine());

            yield return new WaitForEndOfFrame();

            machine.SetState(typeof(PlayerTurnState));
        }
    }

}
