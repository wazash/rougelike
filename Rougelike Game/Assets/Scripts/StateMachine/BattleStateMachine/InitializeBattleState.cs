using System.Collections;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "InitializeBattleState", menuName = "StateMachine/States/InitializeBattleState")]
    public class InitializeBattleState : BattleState
    {
        public override void Enter(GameLoopStateMachine parent)
        {
            //base.Enter(parent);

            CoroutineRunner.Start(BattleInitializationRoutine());
        }

        private IEnumerator BattleInitializationRoutine()
        {
            yield return CoroutineRunner.Start(battleInitRoutines.PrepareDeckRoutine());

            yield return CoroutineRunner.Start(battleInitRoutines.SetEnemiesRoutine());

            yield return new WaitForEndOfFrame();

            machine.SetState(typeof(PlayerTurnState));
        }
    }

}
