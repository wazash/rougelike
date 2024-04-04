using Battle;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "PlayerWinState", menuName = "StateMachine/States/PlayerWinState")]
    public class WinState : BattleState
    {
        private BattleManager battleManager;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            battleManager = parent.GameManager.BattleManager;

            Debug.Log("You win!");

            battleManager.WinScreen.SetActive(true);
        }
    }
}
