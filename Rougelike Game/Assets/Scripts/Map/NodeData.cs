using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public enum NodeType
    {
        Battle,
        Event,
        Shop,
        Treasure,
        Rest,
        Boss
    }

    public enum NodeState
    {
        Locked,
        Unlocked,
        Completed
    }

    public class NodeData
    {
        public string Id { get; set; }
        public NodeType Type { get; set; }
        public NodeState State { get; set; }
        public Vector2 Position { get; set; }
        public List<string> NeighborsIds { get; set; }

        public NodeData() 
        {
            State = NodeState.Locked;
            NeighborsIds = new List<string>();
        }

        public void Unlock()
        {
            switch (Type)
            {
                case NodeType.Battle:
                    // Run battle logic
                    break;
                case NodeType.Event:
                    // Run event logic
                    break;
                case NodeType.Shop:
                    // Run shop logic
                    break;
                case NodeType.Treasure:
                    // Run treasure logic
                    break;
                case NodeType.Rest:
                    // Run rest logic
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            State = NodeState.Completed;
        }

        public void SetState(NodeState state)
        {
            State = state;
        }

        public void AddNeighbor(NodeData neighborNode)
        {
            if(!NeighborsIds.Contains(neighborNode.Id))
            {
                NeighborsIds.Add(neighborNode.Id);
            }
        }
    }
}
