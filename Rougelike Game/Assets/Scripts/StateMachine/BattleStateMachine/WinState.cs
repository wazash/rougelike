using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "PlayerWinState", menuName = "StateMachine/States/PlayerWinState")]
    public class WinState : BattleState
    {
        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            Debug.Log("You win!");
        }
    }
}
