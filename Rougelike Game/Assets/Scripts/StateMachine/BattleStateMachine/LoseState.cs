using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "PlayerLoseState", menuName = "StateMachine/States/PlayerLose")]
    public class LoseState : BattleState
    {
        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            Debug.Log("You Lose");
        }
    }
}
