using Managers;
using Map;
using System;
using System.Collections;
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
            if (parent.PreviousState == machine.GetStateByType(typeof(ChoosePlayerClassState)))
            {
                CoroutineRunner.Start(BuildNewMap());
            }
            else
            {
                CoroutineRunner.Start(SetUpMap());
            }

            mapManager.mapScreen.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            mapManager.mapScreen.SetActive(false);
        }

        private IEnumerator BuildNewMap()
        {
            mapManager.GenerateMap();

            yield return new WaitForEndOfFrame();

            var fistFloorNodes = mapManager.GridGenerator.GetNodesFromFloor(0);
            foreach (NodeData node in fistFloorNodes)
            {
                node.SetState(NodeState.Unlocked);
                node.UIRepresentation.SetNodeData(node);
            }
        }

        private IEnumerator SetUpMap()
        {
            //var currentFloorNodes = mapManager.GridGenerator.GetNodesFromFloor(mapManager.CurrentFloor);
            //foreach (NodeData node in currentFloorNodes)
            //{
            //    node.SetState(NodeState.Unlocked);
            //    node.UIRepresentation.SetNodeData(node);
            //}

            var previouslyClickedNode = mapManager.GridGenerator.GetNodeById(mapManager.CurrentNodeId);
            var previousNodeNeighbours = mapManager.GridGenerator.GetNodeNeighbours(previouslyClickedNode);

            foreach (NodeData node in previousNodeNeighbours)
            {
                if(node.Y > previouslyClickedNode.Y)
                {
                    node.SetState(NodeState.Unlocked);
                    node.UIRepresentation.SetNodeData(node);
                }
            }

            var previousFloorNodes = mapManager.GridGenerator.GetNodesFromFloor(mapManager.CurrentFloor - 1);
            foreach (NodeData node in previousFloorNodes)
            {
                node.SetState(NodeState.Completed);
                node.UIRepresentation.SetNodeData(node);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
