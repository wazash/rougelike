using Managers;
using System.Linq;
using MapGenerator;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "WorldMapState", menuName = "StateMachine/States/WorldMapState")]
    public class WorldMapState : State<GameLoopStateMachine>
    {
        private MapGenerator.MapGenerator mapManager;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            mapManager = GameManager.Instance.MapManager;
            if (parent.PreviousState == parent.MachineStates.First(state => state.GetType() == typeof(ChoosePlayerClassState)))
            {
                SetUpMap();
            }

            mapManager.mapScreen.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            mapManager.mapScreen.SetActive(false);
        }

        private void SetUpMap()
        {
            mapManager.GenerateMap();
        }
    }
}
