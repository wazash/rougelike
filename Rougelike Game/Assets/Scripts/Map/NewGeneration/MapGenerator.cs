using Map;
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

                yield return new WaitForSeconds(1f);
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
                List<int> possibleNextX = GetPossibleNextX(currentNode);

                if (possibleNextX.Count == 0)
                {
                    Debug.LogError("No possible next X");
                    break;
                }

                int nextX = possibleNextX[UnityEngine.Random.Range(0, possibleNextX.Count)];
                int nextY = y + 1;

                NodeData nextNode = nodes[nextX, nextY];
                //if (currentNode.Neighbors.Contains(nextNode) || nextNode.Neighbors.Contains(currentNode)) break;

                currentNode.AddNeighbor(nextNode);

                mapVisualizer.CreateLine(currentNode, nextNode);

                x = nextX;
                y = nextY;
                currentNode = nextNode;
                currentNode.NodeGizmoColor = pathColor;
            }
        }

        private List<int> GetPossibleNextX(NodeData current)
        {
            List<int> possibleNextX = new() { current.X - 1, current.X, current.X + 1 }; // Initial all possible next X

            if (current.X == 0) possibleNextX.Remove(current.X - 1); // Remove left node if current node is at the left edge
            if (current.X == width - 1) possibleNextX.Remove(current.X + 1); // Remove right node if current node is at the right edge

            NodeData leftNode = current.X > 0 ? nodes[current.X - 1, current.Y] : null;
            NodeData rightNode = current.X < width - 1 ? nodes[current.X + 1, current.Y] : null;
            NodeData upNode = current.Y < height - 1 ? nodes[current.X, current.Y + 1] : null;

            if (leftNode != null && leftNode.Neighbors.Contains(upNode)) possibleNextX.Remove(current.X - 1); // Remove up left node if it's connected to up node
            if (rightNode != null && rightNode.Neighbors.Contains(upNode)) possibleNextX.Remove(current.X + 1);// Remove up right node if it's connected to up node

            return possibleNextX;
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
