using Managers;
using NewSaveSystem;
using StateMachine.BattleStateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Node : MonoBehaviour, ISaveable
    {
        [field: SerializeField] public string Id { get; private set; }
        private int x, y;
        [SerializeField] private Vector2 position;
        [SerializeField] private NodeType type;
        [SerializeField] private NodeState state;
        [SerializeField] private List<string> neighborsId;

        [SerializeField] private NodeVisual visual;

        public Vector2 Position => position;

        public NodeState State { get => state; set => state = value; }

        public void SetNodeData(NodeData data)
        {
            Id = data.Id;
            x = data.X;
            y = data.Y;
            position = data.Position;
            type = data.Type;
            state = data.State;
            neighborsId = new();
            foreach (var neighborId in data.NeighborsIds)
            {
                neighborsId.Add(neighborId);
            }

            visual.UpdateVisual(this);
        }

        public string GetSaveID() => Id;
        public Type GetDataType() => typeof(NodeData);

        public object Save()
        {
            return new NodeData()
            {
                Id = Id,
                X = x,
                Y = y,
                Position = position,
                Type = type,
                State = state,
                NeighborsIds = neighborsId
            };
        }

        public void Load(object saveData)
        {
            NodeData nodeData = (NodeData)saveData;
            SetNodeData(nodeData);
        }

        public virtual void NodeRunner()
        {
            BattleState.firstTime = true;
            GameManager.Instance.MapManager.CurrentFloor = y;
            GameManager.Instance.MapManager.CurrentNodeId = Id;
        }
    }
}
