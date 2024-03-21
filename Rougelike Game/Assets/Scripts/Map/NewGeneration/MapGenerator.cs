using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        public GameObject nodeUIPrefab;
        public GameObject lineUIPrefab;
        public Transform mapContainer;
        public NodeData[,] nodes;

        public int width = 7;
        public int height = 15;

        private MapVisualizer mapVisualizer;

        private void Start()
        {
            mapVisualizer = new MapVisualizer(nodeUIPrefab, mapContainer, lineUIPrefab);
            StartCoroutine(GenerateMap());
        }

        private IEnumerator GenerateMap()
        {
            InitializeGrid();

            HashSet<int> usedStartPositions = new();
            NodeData previousPathStartNode = null;

            for (int i = 0; i < width - 1; i++)
            {
                if (previousPathStartNode != null)
                {
                    ChangePathColor(previousPathStartNode, Color.grey);
                }

                int startX;

                if (i < 2)
                {
                    do
                    {
                        startX = UnityEngine.Random.Range(0, width);
                    } while (usedStartPositions.Contains(startX));
                }
                else
                {
                    startX = UnityEngine.Random.Range(0, width);
                }

                usedStartPositions.Add(startX);

                NodeData startNode = nodes[startX, 0];
                previousPathStartNode = startNode;
                CreatePathFrom(startX, 0, Color.green);

                yield return new WaitForSeconds(.2f);
            }

            RemoveUnconnectedNodes();
            UpdateNodeUIs();
        }

        private void InitializeGrid()
        {
            nodes = new NodeData[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new NodeData(x, y);
                }
            }
        }

        private void UpdateNodeUIs()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    NodeData node = nodes[x, y];
                    if (node != null && node.Neighbors.Count > 0)
                    {
                        mapVisualizer.InstantiateNodeUI(node);
                    }
                }
            }
        }

        private void CreatePathFrom(int startX, int startY, Color pathColor)
        {
            int x = startX;
            int y = startY;

            NodeData currentNode = nodes[x, y];
            currentNode.NodeGizmoColor = pathColor;

            while (y < height - 1)
            {
                List<int> possibleNextX = new() { x };

                if (x > 0 && !IsPathCrossing(x, y, x - 1)) possibleNextX.Add(x - 1);
                if (x < width - 1 && !IsPathCrossing(x, y, x + 1)) possibleNextX.Add(x + 1);

                if (possibleNextX.Count == 0)
                {
                    Debug.LogError("No possible next X");
                    break;
                }

                int nextX = possibleNextX[UnityEngine.Random.Range(0, possibleNextX.Count)];
                int nextY = y + 1;

                NodeData nextNode = nodes[nextX, nextY];
                if (currentNode.Neighbors.Contains(nextNode) || nextNode.Neighbors.Contains(currentNode)) break;

                currentNode.AddNeighbor(nextNode);

                mapVisualizer.CreateLine(currentNode, nextNode);

                x = nextX;
                y = nextY;
                currentNode = nextNode;
                currentNode.NodeGizmoColor = pathColor;
            }
        }


        private bool IsPathCrossing(int x, int y, int nextX)
        {
            if (nextX == x) return false;

            NodeData currentNode = nodes[x, y];
            NodeData targetNode = nodes[nextX, y + 1];

            if(nextX > x && x < width - 1)
            {
                if (currentNode.NeighborsIds.Contains(nodes[x + 1, y].Id)) return true;
            }

            if (nextX < x && x > 0)
            {
                if (currentNode.NeighborsIds.Contains(nodes[x - 1, y].Id)) return true;
            }

            if (nextX > x && nextX > 1)
            {
                if (y < height - 2 && nodes[nextX - 1, y + 2] != null && targetNode.NeighborsIds.Contains(nodes[nextX - 1, y + 2].Id))
                {
                    return true;
                }
            }
            if (nextX < x && nextX < width - 2)
            {
                if (y < height - 2 && nodes[nextX + 1, y + 2] != null && targetNode.NeighborsIds.Contains(nodes[nextX + 1, y + 2].Id))
                {
                    return true;
                }
            }

            return false;
        }

        private void ChangePathColor(NodeData startNode, Color pathColor)
        {
            if (startNode == null) return;

            Queue<NodeData> nodesToVisit = new();
            HashSet<NodeData> visitedNodes = new();
            nodesToVisit.Enqueue(startNode);

            while (nodesToVisit.Count > 0)
            {
                NodeData currentNode = nodesToVisit.Dequeue();
                if (visitedNodes.Contains(currentNode)) continue;

                currentNode.NodeGizmoColor = pathColor;
                visitedNodes.Add(currentNode);

                foreach (NodeData neighbor in currentNode.Neighbors)
                {
                    if (neighbor != null && !visitedNodes.Contains(neighbor))
                    {
                        nodesToVisit.Enqueue(neighbor);
                    }
                }
            }
        }

        private void RemoveUnconnectedNodes()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    NodeData node = nodes[x, y];
                    if (node.Neighbors.Count == 0)
                    {
                        nodes[x, y] = null;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (nodes == null)
                return;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    NodeData node = nodes[x, y];
                    if (node != null)
                    {
                        //Gizmos.color = GetColorForNodeType(node.Type);
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(new Vector3(node.X, node.Y, 0), 0.1f);

                        foreach (NodeData neighbor in node.Neighbors)
                        {
                            Gizmos.color = node.NodeGizmoColor;
                            if (neighbor != null)
                                Gizmos.DrawLine(new Vector3(node.X, node.Y, 0), new Vector3(neighbor.X, neighbor.Y, 0));
                        }
                    }
                }
            }
        }
    }
}
