using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "WorldMapState", menuName = "StateMachine/States/WorldMapState")]
    public class WorldMapState : State<GameLoopStateMachine>
    {
        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);
        }
    }

}
