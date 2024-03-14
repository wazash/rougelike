using System.Collections;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "EnemyTurnState", menuName = "StateMachine/States/EnemyTurn")]
    public class EnemyTurnState : BattleState
    {
        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            Debug.Log("Enemy Turn");

            CoroutineRunner.Start(EnterRoutine());
        }

        private IEnumerator EnterRoutine()
        {
            for (int i = 0; i < 3; i++) 
            {
                yield return new WaitForSeconds(1);
            }

            machine.SetState(typeof(PlayerTurnState));
        }
    }
}
