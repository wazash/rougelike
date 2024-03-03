using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "BattleInitializationState", menuName = "StateMachine/States/BattleInitState")]
    internal class BattleInitializationState : State<BattleStateMachine>
    {
        private DeckConfiguration deckConfiguration;
        private Card cardPrefab;
        private Transform gameplayDeckTransform;

        public override void Enter(BattleStateMachine parent)
        {
            base.Enter(parent);

            GetRequiredData(parent);

            CoroutineRunner.Start(InitializeGameplayDeckRoutine(parent));
        }

        private void GetRequiredData(BattleStateMachine parent)
        {
            deckConfiguration = parent.GameManager.DeckConfiguration;
            cardPrefab = parent.GameManager.CardPrefab;
            gameplayDeckTransform = parent.GameManager.DeckPositions.GameplayDeckTransform;
        }

        private IEnumerator InitializeGameplayDeckRoutine(BattleStateMachine parent)
        {
            yield return new WaitForEndOfFrame();

            parent.DeckManager.InitializeMainDeck(deckConfiguration, cardPrefab, gameplayDeckTransform);

            yield return new WaitForEndOfFrame();

            parent.DeckManager.PrepareGameplayDeck();

            yield return new WaitForEndOfFrame();

            machine.SetState(typeof(PlayerTurnState));
        }
    }
}
