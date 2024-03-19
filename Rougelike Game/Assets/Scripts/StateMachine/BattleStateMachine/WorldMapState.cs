using Managers;
using Map;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "WorldMapState", menuName = "StateMachine/States/WorldMapState")]
    public class WorldMapState : State<GameLoopStateMachine>
    {
        [SerializeField] private MapGenerationData mapGenerationData;

        private MapManager mapManager;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            mapManager = GameManager.Instance.MapManager;
            if(parent.PreviousState == parent.MachineStates.First(state => state.GetType() == typeof(ChoosePlayerClassState))) 
            {
                SetUpMap();
            }

            mapManager.MapScreen.SetActive(true);
        }

        private void SetUpMap()
        {
            mapManager.SetMapGenerationStartegy(mapGenerationData.MapStrategy);
            mapManager.GenerateMap(mapGenerationData.MapStrategy, mapGenerationData);
        }
    }
}
