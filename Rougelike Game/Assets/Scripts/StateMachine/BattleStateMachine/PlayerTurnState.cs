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

            CoroutineRunner.Start(EnterRoutine(parent));
        }

        private IEnumerator EnterRoutine(BattleStateMachine parent)
        {
            drawingCoroutine = CoroutineRunner.Start(parent.DeckManager.DrawCardToHand(5));
            yield return drawingCoroutine;
        }
    }
}
