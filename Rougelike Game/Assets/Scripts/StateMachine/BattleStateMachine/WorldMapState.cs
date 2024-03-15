using Managers;
using Map;
using System.Collections;
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

            CoroutineRunner.Start(SetUpMap());

            mapManager.MapScreen.SetActive(true);
        }

        private IEnumerator SetUpMap()
        {
            mapManager.SetMapGenerationStartegy(mapGenerationData.MapStrategy);
            yield return new WaitForEndOfFrame();
            mapManager.GenerateMap(mapGenerationData.MapStrategy, mapGenerationData);
        }
    }

}
