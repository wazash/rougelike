using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class NodeData
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public NodeType Type { get; set; }
        public NodeState State { get; set; }
        [JsonIgnore] public Vector2 Position { get; set; }
        [JsonIgnore] public List<NodeData> Neighbors { get; set; }
        public List<string> NeighborsIds { get; set; }
        [JsonIgnore] public Node UIRepresentation { get; set; }

        public NodeData(int x, int y)
        {
            X = x;
            Y = y;

            State = NodeState.Locked;
            Neighbors = new List<NodeData>();
            NeighborsIds = new List<string>();
            Type = NodeType.Empty;
            UIRepresentation = null;
        }

        public NodeData()
        {
            State = NodeState.Locked;
            Neighbors = new List<NodeData>();
            NeighborsIds = new List<string>();
            Type = NodeType.Empty;
            UIRepresentation = null;
        }

        public void SetState(NodeState state)
        {
            State = state;
        }

        public void AddNeighbor(NodeData neighbor)
        {
            if (neighbor != null && !Neighbors.Contains(neighbor))
            {
                Neighbors.Add(neighbor);
                neighbor.Neighbors.Add(this);

                NeighborsIds.Add(neighbor.Id);
                neighbor.NeighborsIds.Add(Id);
            }
        }
    }
}
