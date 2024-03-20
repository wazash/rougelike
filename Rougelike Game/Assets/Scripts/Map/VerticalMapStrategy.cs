using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Map
{
    public class VerticalMapStrategy : IMapGeneratorStrategy
    {
        private NodeData bossNode;
        private readonly List<NodeData> startNodes;
        public List<List<NodeData>> floors;
        private readonly int maxFloors;
        private readonly int maxBranches;
        private readonly int minNodesOnFloor;
        private readonly int maxNodesOnFloor;

        private bool isTreasureGenerated;
        private bool isFirstEliteGenerated;
        private NodeTypeProbabilityManager probabilityManager;
        private Dictionary<NodeType, float> baseProbabilities;

        public VerticalMapStrategy(int maxFloors, int maxBranches, MinMaxInt nodesOnFloor)
        {
            this.maxFloors = maxFloors;
            this.maxBranches = maxBranches;
            minNodesOnFloor = nodesOnFloor.MinValue;
            maxNodesOnFloor = nodesOnFloor.MaxValue;

            floors = new List<List<NodeData>>();
            startNodes = new List<NodeData>();

            isTreasureGenerated = false;
            isFirstEliteGenerated = false;

            baseProbabilities = new Dictionary<NodeType, float>
            {
                { NodeType.Battle, 0.5f },
                { NodeType.Event, 0.25f },
                { NodeType.Shop, 0.15f },
                { NodeType.Elite, 0.05f },
                { NodeType.Treasure, 0.04f },
                { NodeType.Rest, 0.01f }
            };

            probabilityManager = new NodeTypeProbabilityManager(baseProbabilities);
        }

        public List<NodeData> GenerateMap(int startPathsCount, float branchProbability)
        {
            GenerateFirstFloor(startPathsCount);
            GenerateRestOfFloors(branchProbability);
            AddBossNode();

            return ReturnAllNodes();
        }

        private void GenerateFirstFloor(int startPathsCount)
        {
            for (int i = 0; i < startPathsCount; i++)
            {
                NodeData newNode = new()
                {
                    Id = $"StartNode_0_{i}",
                    Type = NodeType.Battle
                };
                startNodes.Add(newNode);
            }
            floors.Add(startNodes);
        }

        private void GenerateRestOfFloors(float branchProbability)
        {
            // Generate nodes for the rest of the floors
            for (int floor = 1; floor < maxFloors; floor++)
            {
                List<NodeData> previousFloor = floors[floor - 1];
                List<NodeData> currentFloor = new();

                GenerateCurrentFloorNodes(floor, currentFloor, ref isFirstEliteGenerated, ref isTreasureGenerated);

                // Connect nodes from the previous floor to the current floor
                for (int parentNodeIndex = 0; parentNodeIndex < previousFloor.Count; parentNodeIndex++)
                {
                    GetPreviousFloorClosestNodesIndices(previousFloor, parentNodeIndex, out NodeData parentNode, out int[] closestNodesIndices);
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
                    if (currentFloor[i]./*Neighbors*/NeighborsIds.Count == 0)
                    {
                        ConnectCurrentFloorNodeToNodeBelow(previousFloor, currentFloor, i);
                    }
                }

                floors.Add(currentFloor);
            }
        }

        private NodeType GetRandomTypeWithPreference()
        {
            var probabilities = new List<NodeProbablity>
            {
                new(NodeType.Battle, 0.4f),
                new(NodeType.Event, 0.3f),
                new(NodeType.Shop, 0.2f),
                new(NodeType.Elite, 0.05f),
                new(NodeType.Treasure, 0.04f),
                new(NodeType.Rest, 0.01f)
            };

            var randomizer = new NodeTypeRandomizer(probabilities);
            return randomizer.GetRandomNodeType();
        }

        private void GenerateCurrentFloorNodes(int floor, List<NodeData> currentFloor, ref bool isFirstEliteGenerated, ref bool isTreasureGenerated)
        {
            int nodesOnFloor = Random.Range(minNodesOnFloor, maxNodesOnFloor + 1);

            for (int i = 0; i < nodesOnFloor; i++)
            {
                NodeType nodeType;

                if(StartingNodesCondition(floor))
                {
                    nodeType = NodeType.Battle;
                }
                else if (EliteFloorCondition(floor))
                {
                    nodeType = NodeType.Elite;
                }
                else if (TreasureNodeCondition(floor, ref isFirstEliteGenerated, ref isTreasureGenerated))
                {
                    nodeType = NodeType.Treasure;
                }
                else if (LastFloorCondition(floor))
                {
                    nodeType = NodeType.Rest;
                }
                else
                {
                    nodeType = probabilityManager.GetRandomNodeType();
                }

                NodeData newNode = new()
                {
                    Id = $"Node_{floor}_{i}",
                    Type = nodeType
                };

                if(nodeType == NodeType.Elite)
                {
                    isFirstEliteGenerated = true;
                }
                else if(nodeType == NodeType.Treasure)
                {
                    isTreasureGenerated = true;
                }

                currentFloor.Add(newNode);
                probabilityManager.UpdateSpawnCount(nodeType);
            }

            probabilityManager.IncrementFloor();
        }

        private bool StartingNodesCondition(int floor) => floor == 0;
        private bool EliteFloorCondition(int floor) => floor == maxFloors / 3 || floor == 2 * maxFloors / 3;
        private bool TreasureNodeCondition(int floor, ref bool isFirstEliteGenerated, ref bool isTreasureGenerated) 
            => !isTreasureGenerated && isFirstEliteGenerated && floor > maxFloors / 3;
        private bool LastFloorCondition(int floor) => floor == maxFloors - 1;

        private void GetPreviousFloorClosestNodesIndices(List<NodeData> previousFloor, int parentNodeIndex, out NodeData parentNode, out int[] closestNodesIndices)
        {
            parentNode = previousFloor[parentNodeIndex];
            closestNodesIndices = new int[maxBranches];
            int mostLeftIndex = parentNodeIndex - maxBranches / 2;

            for (int i = 0; i < maxBranches; i++)
            {
                closestNodesIndices[i] = mostLeftIndex + i;
            }
        }

        private bool ConnectParentNodeToClosestChildNodes(float branchProbability, List<NodeData> currentFloor, NodeData parentNode, int[] closestNodesIndices)
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

        private static void ConnectParentNodeToRandomClostestChildNode(List<NodeData> currentFloor, NodeData parentNode, int[] closestNodesIndices)
        {
            int randomIndex = closestNodesIndices[Random.Range(0, closestNodesIndices.Length)];
            randomIndex = Mathf.Clamp(randomIndex, 0, currentFloor.Count - 1);
            parentNode.AddNeighbor(currentFloor[randomIndex]);
            currentFloor[randomIndex].AddNeighbor(parentNode);
        }

        private static void ConnectCurrentFloorNodeToNodeBelow(List<NodeData> previousFloor, List<NodeData> currentFloor, int i)
        {
            int index = i;
            index = Mathf.Clamp(index, 0, previousFloor.Count - 1); // Clamp the index
            currentFloor[i].AddNeighbor(previousFloor[index]);
            previousFloor[index].AddNeighbor(currentFloor[i]);
        }

        private void AddBossNode()
        {
            // Add boss node
            bossNode = new()
            {
                Id = "BossNode",
                Type = NodeType.Boss
            };
            foreach (var lastFloorNode in floors[^1])
            {
                lastFloorNode.AddNeighbor(bossNode);
                bossNode.AddNeighbor(lastFloorNode);
            }
        }

        private List<NodeData> ReturnAllNodes()
        {
            // Create a list of all nodes
            List<NodeData> allNodes = new();
            foreach (var floor in floors)
            {
                allNodes.AddRange(floor);
            }
            allNodes.Add(bossNode);

            return allNodes;
        }

        public void CalculateNodePositions(RectTransform mapContainer)
        {
            float verticalSpacing = 250.0f; // Distance between floors
            float containerWidth = mapContainer.rect.width;
            float containerHeight = mapContainer.rect.height;

            // Calculate start nodes positions
            float horizontalSpacingStart = containerWidth / (startNodes.Count + 1); // Distance between start nodes
            for (int i = 0; i < startNodes.Count; i++)
            {
                float xPosition = horizontalSpacingStart * (i + 1) - (containerWidth / 2);
                float yPosition = -containerHeight / 2 + verticalSpacing;
                Vector2 position = new(xPosition, yPosition);
                startNodes[i].Position = position;
            }

            // Calculate the rest of the floors
            for (int floorIndex = 1; floorIndex < floors.Count; floorIndex++)
            {
                List<NodeData> currentFloor = floors[floorIndex];
                float horizontalSpacing = containerWidth / (currentFloor.Count + 1);

                for (int nodeIndex = 0; nodeIndex < currentFloor.Count; nodeIndex++)
                {
                    float xPosition = horizontalSpacing * (nodeIndex + 1) - (containerWidth / 2);
                    float yPosition = -containerHeight / 2 + verticalSpacing + (verticalSpacing * floorIndex);
                    Vector2 position = new(xPosition, yPosition);
                    currentFloor[nodeIndex].Position = position;
                }
            }

            // Calculate boss node position
            if (bossNode != null)
            {
                Vector2 position = new(0, -containerHeight / 2 + verticalSpacing + (verticalSpacing * floors.Count));
                bossNode.Position = position;
            }
        }
    }
}
