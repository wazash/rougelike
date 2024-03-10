using Cards;
using System.Collections;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "BattleInitializationState", menuName = "StateMachine/States/BattleInitState")]
    internal class BattleInitializationState : State<BattleStateMachine>
    {
        private StartingDeckConfig deckConfiguration;
        private Card cardPrefab;
        private Transform gameplayDeckTransform;

        public override void Enter(BattleStateMachine parent)
        {
            base.Enter(parent);

            GetRequiredData();

            CoroutineRunner.Start(InitializeGameplayDeckRoutine());
        }

        private void GetRequiredData()
        {
            deckConfiguration = machine.GameManager.DeckConfiguration;
            cardPrefab = machine.GameManager.CardPrefab;
            gameplayDeckTransform = machine.GameManager.DeckPositions.GameplayDeckTransform;
        }

        private IEnumerator InitializeGameplayDeckRoutine()
        {
            yield return new WaitForEndOfFrame();

            machine.DeckManager.InitializeMainDeck(deckConfiguration, cardPrefab, gameplayDeckTransform);

            yield return new WaitForEndOfFrame();

            machine.DeckManager.PrepareGameplayDeck();

            yield return new WaitForEndOfFrame();

            machine.SetState(typeof(PlayerTurnState));
        }
    }
}
