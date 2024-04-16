using Battle;
using Managers;
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

        public void ContinueWinButton()
        {
            battleManager.WinScreen.SetActive(false);
            battleManager.BattleScreen.SetActive(false);

            GameManager.Instance.MapManager.CurrentFloor++;

            machine.SetState(typeof(WorldMapState));
        }
    }
}
