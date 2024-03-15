using System;
using System.Collections.Generic;

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

    public class Node
    {
        public string Id { get; private set; }
        public NodeType Type { get; private set; }
        public NodeState State { get; private set; }
        public List<Node> Neighbors { get; private set; }

        public Node(string id, NodeType type)
        {
            Id = id;
            Type = type;
            State = NodeState.Locked;
            Neighbors = new List<Node>();
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

        public void AddNeighbor(Node neighborNode)
        {
            if (!Neighbors.Contains(neighborNode))
            {
                Neighbors.Add(neighborNode);
                //neighborNode.AddNeighbor(this);
            }
        }
    }
}
