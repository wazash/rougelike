using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class VerticalMapStrategy : IMapGeneratorStrategy
    {
        private Node bossNode;
        private List<Node> startNodes;
        public List<List<Node>> floors; // Every floor is a list of nodes
        private readonly int maxFloors;
        private readonly int maxBranches;
        private readonly int minNodesOnFloor;
        private readonly int maxNodesOnFloor;

        public VerticalMapStrategy(int maxFloors, int maxBranches, int minNodesOnFloor, int maxNodesOnFloor)
        {
            this.maxFloors = maxFloors;
            this.maxBranches = maxBranches;
            this.minNodesOnFloor = minNodesOnFloor;
            this.maxNodesOnFloor = maxNodesOnFloor;

            floors = new List<List<Node>>();
            startNodes = new List<Node>();
        }

        public List<Node> GenerateMap(int startPathsCount, float branchProbability)
        {
            GenerateFirstFloor(startPathsCount);
            GenerateRestOfFloors(branchProbability);
            AddBossNode();

            return ReturnAllNodes();
        }

        private void GenerateFirstFloor(int startPathsCount)
        {
            List<Node> firstFloor = new();
            for (int i = 0; i < startPathsCount; i++)
            {
                Node newNode = new($"Node_0_{i}", NodeType.Battle);
                firstFloor.Add(newNode);
                startNodes.Add(newNode);
            }
            floors.Add(firstFloor);
        }

        private void GenerateRestOfFloors(float branchProbability)
        {
            // Generate nodes for the rest of the floors
            for (int floor = 1; floor < maxFloors; floor++)
            {
                List<Node> previousFloor = floors[floor - 1];
                List<Node> currentFloor = new();

                GenerateCurrentFloorNodes(floor, currentFloor);

                // Connect nodes from the previous floor to the current floor
                for (int parentNodeIndex = 0; parentNodeIndex < previousFloor.Count; parentNodeIndex++)
                {
                    GetPreviousFloorClosestNodesIndices(previousFloor, parentNodeIndex, out Node parentNode, out int[] closestNodesIndices);
                    bool hasConnection = ConnectParentNodeToClosestChildNodes(branchProbability, currentFloor, parentNode, closestNodesIndices);

                    // If there is no connection, connect random node from the closest nodes
                    if (!hasConnection)
                    {
                        ConnectParentNodeToRandomClostestChildNode(currentFloor, parentNode, closestNodesIndices);
                    }
                }

                // Check if there are nodes on the current floor that are not connected to any other node and connect them to the previous floor
                for (int i = 0; i < currentFloor.Count; i++)
                {
                    if (currentFloor[i].Neighbors.Count == 0)
                    {
                        ConnectCurrentFloorNodeToNodeBelow(previousFloor, currentFloor, i);
                    }
                }

                floors.Add(currentFloor);
            }
        }

        private void GenerateCurrentFloorNodes(int floor, List<Node> currentFloor)
        {
            int nodesOnFloor = Random.Range(minNodesOnFloor, maxNodesOnFloor); 
            for (int i = 0; i < nodesOnFloor; i++)
            {
                Node newNode = new($"Node_{floor}_{i}", NodeType.Battle); 
                currentFloor.Add(newNode); 
            }
        }

        private void GetPreviousFloorClosestNodesIndices(List<Node> previousFloor, int parentNodeIndex, out Node parentNode, out int[] closestNodesIndices)
        {
            parentNode = previousFloor[parentNodeIndex]; 
            closestNodesIndices = new int[maxBranches]; 
            int mostLeftIndex = parentNodeIndex - maxBranches / 2; 

            for (int i = 0; i < maxBranches; i++)
            {
                closestNodesIndices[i] = mostLeftIndex + i;
            }
        }

        private bool ConnectParentNodeToClosestChildNodes(float branchProbability, List<Node> currentFloor, Node parentNode, int[] closestNodesIndices)
        {
            if (closestNodesIndices.Length == 0 || currentFloor.Count == 0)
                return false;

            bool hasConnection = false;

            int connectionAttempts = (int)(maxBranches * branchProbability);

            for (int attempt = 0; attempt < connectionAttempts; attempt++)
            {
                foreach (int index in closestNodesIndices)
                {
                    if (index < 0 || index >= currentFloor.Count)
                        continue;

                    parentNode.AddNeighbor(currentFloor[index]);
                    currentFloor[index].AddNeighbor(parentNode);
                    hasConnection = true;
                }
            }

            return hasConnection;
        }

        private static void ConnectParentNodeToRandomClostestChildNode(List<Node> currentFloor, Node parentNode, int[] closestNodesIndices)
        {
            int randomIndex = closestNodesIndices[Random.Range(0, closestNodesIndices.Length)];
            randomIndex = Mathf.Clamp(randomIndex, 0, currentFloor.Count - 1);
            parentNode.AddNeighbor(currentFloor[randomIndex]);
            currentFloor[randomIndex].AddNeighbor(parentNode);
        }

        private static void ConnectCurrentFloorNodeToNodeBelow(List<Node> previousFloor, List<Node> currentFloor, int i)
        {
            int index = i;
            index = Mathf.Clamp(index, 0, previousFloor.Count - 1); // Clamp the index
            currentFloor[i].AddNeighbor(previousFloor[index]);
            previousFloor[index].AddNeighbor(currentFloor[i]);
        }

        private void AddBossNode()
        {
            // Add boss node
            bossNode = new($"BossNode", NodeType.Boss);
            foreach (var lastFloorNode in floors[^1])
            {
                lastFloorNode.AddNeighbor(bossNode);
                bossNode.AddNeighbor(lastFloorNode);
            }
        }

        private List<Node> ReturnAllNodes()
        {
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
