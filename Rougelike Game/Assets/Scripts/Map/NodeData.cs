using System;
using System.Collections.Generic;
using System.Linq;
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
        Boss,
        Elite
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

    public class NodeProbablity
    {
        public NodeType Type { get; }
        public float Probability { get; }

        public NodeProbablity(NodeType type, float probability)
        {
            Type = type;
            Probability = probability;
        }
    }

    public class NodeTypeRandomizer
    {
        private readonly List<NodeProbablity> nodeProbablities;

        public NodeTypeRandomizer(IEnumerable<NodeProbablity> nodeProbablities)
        {
            this.nodeProbablities = new List<NodeProbablity>(nodeProbablities);
        }

        public NodeType GetRandomNodeType()
        {
            float totalProbability = nodeProbablities.Sum(node => node.Probability);
            float randomPoint = UnityEngine.Random.Range(0, totalProbability);

            float cumulativeProbability = 0;
            foreach(var nodeProbability in nodeProbablities)
            {
                cumulativeProbability += nodeProbability.Probability;
                if (randomPoint <= cumulativeProbability)
                {
                    return nodeProbability.Type;
                }
            }

            return nodeProbablities.First().Type;
        }
    }

    public class NodeTypeProbabilityManager
    {
        private Dictionary<NodeType, float> baseProbabilities;
        private Dictionary<NodeType, int> spawnCount;
        private const float DECREASE_FACTOR = 0.5f;
        private const float INCREASE_FACTOR = 0.1f;

        public NodeTypeProbabilityManager (Dictionary<NodeType, float> baseProbabilities)
        {
            this.baseProbabilities = new Dictionary<NodeType, float>(baseProbabilities);
            spawnCount = new Dictionary<NodeType, int>();

            foreach (var nodeType in baseProbabilities.Keys)
            {
                spawnCount[nodeType] = 0;
            }
        }

        public NodeType GetRandomNodeType()
        {
            var adjustedProbabilities = new List<NodeProbablity>();
            float totalProbability = 0;

            foreach (var entry in baseProbabilities)
            {
                float adjustedProbability = entry.Value;
                int count = spawnCount[entry.Key];

                adjustedProbability *= (float)Math.Pow(1 -DECREASE_FACTOR, count);
                adjustedProbability += INCREASE_FACTOR * (1 - adjustedProbability);

                adjustedProbabilities.Add(new NodeProbablity(entry.Key, adjustedProbability));
                totalProbability += adjustedProbability;
            }

            float randomPoint = UnityEngine.Random.Range(0, totalProbability);
            float cumulativeProbability = 0;

            foreach (var probability in adjustedProbabilities)
            {
                cumulativeProbability += probability.Probability;
                if (randomPoint <= cumulativeProbability)
                {
                    spawnCount[probability.Type]++;
                    return probability.Type;
                }
            }

            return adjustedProbabilities.First().Type;
        }

        public void UpdateSpawnCount(NodeType nodeType)
        {
            if(spawnCount.ContainsKey(nodeType))
            {
                spawnCount[nodeType]++;
            }
        }

        public void ResetSpawnCount()
        {
            foreach (var nodeType in spawnCount.Keys)
            {
                spawnCount[nodeType] = 0;
            }
        }

        public void IncrementFloor()
        {
            foreach (var type in spawnCount.Keys.ToList())
            {
                spawnCount[type] = Math.Max(0, spawnCount[type] - 1);
            }
        }
    }
}
