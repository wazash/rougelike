using Map;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    public class MapVisualizer
    {
        private NodeGridGenerator gridGenerator;
        private List<NodeTypeScriptableObject> nodeUIPrefabs;
        private GameObject lineUIPrefab;
        private Transform mapContainer;
        private Transform pathsContainer;

        public MapVisualizer(NodeGridGenerator gridGenerator, List<NodeTypeScriptableObject> nodeUIPrefabs, GameObject lineUIPrefab, Transform mapContainer, Transform pathsContainer)
        {
            this.gridGenerator = gridGenerator;
            this.nodeUIPrefabs = nodeUIPrefabs;
            this.lineUIPrefab = lineUIPrefab;
            this.mapContainer = mapContainer;
            this.pathsContainer = pathsContainer;
        }

        public void VisualizeMap()
        {
            for (int x = 0; x < gridGenerator.Width; x++)
            {
                for (int y = 0; y < gridGenerator.Height; y++)
                {
                    NodeData node = gridGenerator.Nodes[x, y];
                    if (node == null || node.NeighborsIds.Count == 0) continue;

                    node.X = x;
                    node.Y = y;
                    InstantiateNodeUI(node);
                }
            }

            NodeData bossNode = gridGenerator.GetBossNode();
            InstantiateNodeUI(bossNode);
        }

        public void VisualizePaths()
        {
            for (int x = 0; x < gridGenerator.Width; x++)
            {
                for (int y = 0; y < gridGenerator.Height; y++)
                {
                    NodeData node = gridGenerator.Nodes[x, y];
                    if (node == null) continue;

                    foreach (string neighborId in node.NeighborsIds)
                    {
                        NodeData neighbor = gridGenerator.GetNodeById(neighborId);
                        if (neighbor != null)
                            InstantiateLineUI(node, neighbor);
                    }
                }
            }
        }

        private void InstantiateNodeUI(NodeData nodeData)
        {
            Node nodeUIPrefab = GetPrefabBasedOnType(nodeData.Type);

            Node nodeObject = Object.Instantiate(nodeUIPrefab, mapContainer);
            if (nodeData.UIRepresentation == null)
                nodeData.UIRepresentation = nodeObject; // Save the reference to the UI object in the node data

            nodeData.UIRepresentation.transform.localPosition = CalculateNodePosition(nodeData.X, nodeData.Y);
            nodeData.Position = nodeData.UIRepresentation.transform.localPosition;
            nodeObject.name = nodeData.Id;
            nodeData.UIRepresentation.SetNodeData(nodeData);
        }

        private void InstantiateLineUI(NodeData startNode, NodeData endNode)
        {
            GameObject lineUI = Object.Instantiate(lineUIPrefab, pathsContainer);
            lineUI.transform.SetLocalPositionAndRotation(CalculateLinePosition(startNode, endNode), CalculateLineRotation(startNode, endNode));
            lineUI.GetComponent<RectTransform>().sizeDelta = new Vector2(CalculateLineLength(startNode, endNode), 3);
        }

        private Vector2 CalculateNodePosition(int x, int y)
        {
            NodeData nodeData = gridGenerator.Nodes[x, y];
            Node nodeUIPrefab = GetPrefabBasedOnType(nodeData.Type);
            RectTransform nodeRectTransform = nodeUIPrefab.GetComponent<RectTransform>();
            float horizontalSpacing = 40.0f;
            float verticalSpacing = 60.0f;

            float posX = (-(gridGenerator.Width / 2) + x) * (nodeRectTransform.sizeDelta.x + horizontalSpacing);
            float posY;
            if (nodeData.Type == NodeType.Boss)
            {
                posY = gridGenerator.Height * (125 + verticalSpacing) + verticalSpacing * 2;
            }
            else
            {
                posY = (1 + y) * (nodeRectTransform.sizeDelta.y + verticalSpacing);
            }
            return new Vector2(posX, posY);
        }

        private Node GetPrefabBasedOnType(NodeType type)
        {
            foreach (NodeTypeScriptableObject nodeType in nodeUIPrefabs)
            {
                if (nodeType.NodeType == type)
                    return nodeType.NodePrefab;
            }

            return nodeUIPrefabs[0].NodePrefab;
        }

        private Vector2 CalculateLinePosition(NodeData startNode, NodeData endNode)
        {
            Vector2 startPosition = CalculateNodePosition(startNode.X, startNode.Y);
            Vector2 endPosition = CalculateNodePosition(endNode.X, endNode.Y);

            return (startPosition + endPosition) / 2;
        }

        private Quaternion CalculateLineRotation(NodeData startNode, NodeData endNode)
        {
            Vector2 startPosition = CalculateNodePosition(startNode.X, startNode.Y);
            Vector2 endPosition = CalculateNodePosition(endNode.X, endNode.Y);

            Vector2 direction = endPosition - startPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(0, 0, angle);
        }

        private float CalculateLineLength(NodeData startNode, NodeData endNode)
        {
            Vector2 startPosition = CalculateNodePosition(startNode.X, startNode.Y);
            Vector2 endPosition = CalculateNodePosition(endNode.X, endNode.Y);

            return Vector2.Distance(startPosition, endPosition);
        }
    }
}
