using Map;
using UnityEngine;

namespace TestGenerator
{
    public class MapVisualizer
    {
        private NodeGridGenerator gridGenerator;
        private Node nodeUIPrefab;
        private GameObject lineUIPrefab;
        private Transform mapContainer;
        private Transform pathsContainer;

        public MapVisualizer(NodeGridGenerator gridGenerator, Node nodeUIPrefab, GameObject lineUIPrefab, Transform mapContainer, Transform pathsContainer)
        {
            this.gridGenerator = gridGenerator;
            this.nodeUIPrefab = nodeUIPrefab;
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
                    if (node == null || node.Neighbors.Count <= 0) continue;
                    
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

                    foreach (NodeData neighbor in node.Neighbors)
                    {
                        if (neighbor == null) continue;

                        InstantiateLineUI(node, neighbor);
                    }
                }
            }
        }

        private void InstantiateNodeUI(NodeData node)
        {
            Node nodeObject = Object.Instantiate(nodeUIPrefab, mapContainer);
            nodeObject.transform.localPosition = CalculateNodePosition(node.X, node.Y);
            node.UIRepresentation = nodeObject; // Save the reference to the UI object in the node data

            nodeObject.GetComponent<Node>().SetNodeData(node);
            nodeObject.name = node.Id;

        }

        private void InstantiateLineUI(NodeData startNode, NodeData endNode)
        {
            GameObject lineUI = Object.Instantiate(lineUIPrefab, pathsContainer);
            lineUI.transform.SetLocalPositionAndRotation(CalculateLinePosition(startNode, endNode), CalculateLineRotation(startNode, endNode));
            lineUI.GetComponent<RectTransform>().sizeDelta = new Vector2(CalculateLineLength(startNode, endNode), 3);
        }

        private Vector2 CalculateNodePosition(int x, int y)
        {
            RectTransform nodeRectTransform = nodeUIPrefab.GetComponent<RectTransform>();
            float horizontalSpacing = 40.0f;
            float verticalSpacing = 60.0f;

            float posX = x * (nodeRectTransform.sizeDelta.x + horizontalSpacing);
            float posY = y * (nodeRectTransform.sizeDelta.y + verticalSpacing);

            return new Vector2(posX, posY);
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
