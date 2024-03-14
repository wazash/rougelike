using System.Collections.Generic;
using UnityEngine;

namespace Map
{

    public class HorizontalMapStrategy : IMapGeneratorStrategy
    {
        public void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer)
        {
            throw new System.NotImplementedException();
        }

        public List<Node> GenerateMap(int numerOdNodes, int startPathsCount, float branchingProbability)
        {
            // Horizontal map generation logic
            return new List<Node>();
        }
    }

    public class RadialMapStrategy : IMapGeneratorStrategy
    {
        public void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer)
        {
            throw new System.NotImplementedException();
        }

        public List<Node> GenerateMap(int numerOdNodes, int startPathsCount, float branchingProbability)
        {
            // Radial map generation logic
            return new List<Node>();
        }
    }

    public class MapGenerator
    {
        private List<Node> allNodes;

        public MapGenerator(List<Node> nodes)
        {
            allNodes = nodes;
        }

        /// <summary>
        /// Generates a map with a given number of nodes, starting paths count and branching probability
        /// </summary>
        /// <param name="numerOdNodes">Number of nodes</param>
        /// <param name="startPathsCount">STarting paths count</param>
        /// <param name="branchingProbability">Branching probability</param>
        /// <returns></returns>
        public List<Node> GenerateMap(int numerOdNodes, int startPathsCount, float branchingProbability)
        {
            allNodes ??= new();

            // Create starting nodes
            for(int i = 0; i < startPathsCount; i++)
            {
                Node newNode = new($"Node_{i}", NodeType.Battle);
                allNodes.Add(newNode);
            }

            // Create the rest of the nodes
            for(int i = startPathsCount; i < numerOdNodes; i++)
            {
                Node newNode = new($"Node_{i}", NodeType.Battle);
                allNodes.Add(newNode);

                foreach(Node existingNode in allNodes)
                {
                    if(Random.value < branchingProbability && existingNode != newNode)
                    {
                        existingNode.AddNeighbor(newNode);
                    }
                }
            }

            return allNodes;
        }
    }
}
