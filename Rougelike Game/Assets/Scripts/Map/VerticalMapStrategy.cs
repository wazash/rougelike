using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class VerticalMapStrategy : IMapGeneratorStrategy
    {
        private Node bossNode;
        private List<Node> startNodes;
        public List<List<Node>> floors; // Every floor is a list of nodes
        private int maxFloors;
        private int maxBranches = 3;
        private int nodeIdCounter = 0;
        private int minNodesOnFloor = 3;
        private int maxNodesOnFloor = 5;

        public VerticalMapStrategy(int maxFloors)
        {
            this.maxFloors = maxFloors;
            floors = new List<List<Node>>();
            startNodes = new List<Node>();
        }

        public List<Node> GenerateMap(int numberOfNodes, int startPathsCount, float branchProbability)
        {
            // Generate the first (starting) floor
            List<Node> firstFloor = new();
            for (int i = 0; i < startPathsCount; i++)
            {
                Node newNode = new($"Node_0_{i}", NodeType.Battle);
                firstFloor.Add(newNode);
                startNodes.Add(newNode);
            }
            floors.Add(firstFloor);

            // Generate nodes for the rest of the floors
            for (int floor = 1; floor < maxFloors; floor++)
            {
                List<Node> previousFloor = floors[floor - 1];
                List<Node> currentFloor = new();

                // Generate nodes for the current floor
                int nodesOnFloor = Random.Range(minNodesOnFloor, maxNodesOnFloor); // Random number of nodes on the current floor
                for (int i = 0; i < nodesOnFloor; i++)
                {
                    Node newNode = new($"Node_{floor}_{i}", NodeType.Battle);
                    currentFloor.Add(newNode);
                }

                // Connect nodes from the previous floor to the current floor
                for (int parentNodeIndex = 0; parentNodeIndex < previousFloor.Count; parentNodeIndex++)
                {
                    Node parentNode = previousFloor[parentNodeIndex]; // Parent node from the previous floor
                    int[] closestNodesIndices = new int[maxBranches]; // Indices of the closest nodes on the current floor
                    int mostLeftIndex = parentNodeIndex - maxBranches / 2; // Index of the most left node from the closest nodes

                    // Fill the closest nodes indices array
                    for (int i = 0; i < maxBranches; i++)
                    {
                        closestNodesIndices[i] = mostLeftIndex + i; // Fill the array with indices
                    }

                    // Connect the parent node to the closest nodes
                    bool hasConnection = false; // Flag to check if there is a connection
                    for(int i = 0; i < maxBranches; i++)
                    {
                        if (Random.value < branchProbability)
                        {
                            foreach (int closestNodeIndex in closestNodesIndices)
                            {
                                if (closestNodeIndex >= 0 && closestNodeIndex < currentFloor.Count)
                                {
                                    parentNode.AddNeighbor(currentFloor[closestNodeIndex]); // Connect the parent node to the closest node
                                    currentFloor[closestNodeIndex].AddNeighbor(parentNode); // Connect the closest node to the parent node
                                    hasConnection = true; // Set the flag to true
                                }
                            }
                        }
                    }

                    // If there is no connection, connect random node from the closest nodes
                    if (!hasConnection)
                    {
                        int randomIndex = closestNodesIndices[Random.Range(0, closestNodesIndices.Length)]; // Random index from the closest nodes
                        randomIndex = Mathf.Clamp(randomIndex, 0, currentFloor.Count - 1); // Clamp the random index
                        parentNode.AddNeighbor(currentFloor[randomIndex]); // Connect the parent node to the closest node
                        currentFloor[randomIndex].AddNeighbor(parentNode); // Connect the closest node to the parent node
                    }
                }

                for(int i = 0; i < currentFloor.Count; i++)
                {
                    if (currentFloor[i].Neighbors.Count == 0)
                    {
                        int index = i;
                        index = Mathf.Clamp(index, 0, previousFloor.Count - 1); // Clamp the index
                        currentFloor[i].AddNeighbor(previousFloor[index]);
                        previousFloor[index].AddNeighbor(currentFloor[i]);
                        Debug.Log($"Node {currentFloor[i].Id} has no neighbors, connecting to {previousFloor[index].Id}");
                    }
                }

                floors.Add(currentFloor);
            }

            // Add boss node
            bossNode = new($"BossNode", NodeType.Boss);
            foreach (var lastFloorNode in floors[^1])
            {
                lastFloorNode.AddNeighbor(bossNode);
            }

            // Create a list of all nodes
            List<Node> allNodes = new(startNodes);
            foreach (var floor in floors)
            {
                allNodes.AddRange(floor);
            }
            allNodes.Add(bossNode);

            return allNodes;
        }

        public void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer)
        {
            float verticalSpacing = 150.0f; // Distance between floors
            float containerWidth = mapContainer.rect.width;
            float containerHeight = mapContainer.rect.height;

            // Calculate start nodes positions
            float horizontalSpacingStart = containerWidth / (startNodes.Count + 1); // Distance between start nodes
            for (int i = 0; i < startNodes.Count; i++)
            {
                float xPosition = horizontalSpacingStart * (i + 1) - (containerWidth / 2);
                float yPosition = -containerHeight / 2 + verticalSpacing;
                nodePositions[startNodes[i].Id] = new Vector2(xPosition, yPosition);
            }

            // Calculate the rest of the floors
            for (int floorIndex = 1; floorIndex < floors.Count; floorIndex++)
            {
                List<Node> currentFloor = floors[floorIndex];
                float horizontalSpacing = containerWidth / (currentFloor.Count + 1);

                for (int nodeIndex = 0; nodeIndex < currentFloor.Count; nodeIndex++)
                {
                    float xPosition = horizontalSpacing * (nodeIndex + 1) - (containerWidth / 2);
                    float yPosition = -containerHeight / 2 + verticalSpacing + (verticalSpacing * floorIndex);
                    nodePositions[currentFloor[nodeIndex].Id] = new Vector2(xPosition, yPosition);
                }
            }

            // Calculate boss node position
            if (bossNode != null)
            {
                nodePositions[bossNode.Id] = new Vector2(0, -containerHeight / 2 + verticalSpacing + (verticalSpacing * floors.Count));
            }
        }
    }
}
