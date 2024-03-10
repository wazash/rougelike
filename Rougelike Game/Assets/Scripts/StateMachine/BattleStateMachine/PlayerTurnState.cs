using System.Collections;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "PlayerTurnState", menuName = "StateMachine/States/PlayerTurn")]
    public class PlayerTurnState : State<BattleStateMachine>
    {
        private Coroutine drawingCoroutine;

        public override void Enter(BattleStateMachine parent)
        {
            base.Enter(parent);

            CoroutineRunner.Start(EnterRoutine());
        }

        public override void Exit()
        {
            base.Exit();

            CoroutineRunner.Start(machine.DeckManager.DiscardAllHandCards());
        }

        private IEnumerator EnterRoutine()
        {
            yield return CoroutineRunner.Start(machine.DeckManager.DrawCardToHand(5, 0.2f));
        }

        public void EndTurn()
        {
            machine.SetState(typeof(EnemyTurnState));
        }
    }
}
