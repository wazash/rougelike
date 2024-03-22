using Map;
using UnityEngine;

namespace TestGenerator
{
    public class MapVisualizer
    {
        private GameObject nodeUIPrefab;
        private GameObject lineUIPrefab;
        private Transform mapContainer;

        public MapVisualizer(GameObject nodeUIPrefab, Transform mapContainer, GameObject lineUIPrefab)
        {
            this.nodeUIPrefab = nodeUIPrefab;
            this.mapContainer = mapContainer;
            this.lineUIPrefab = lineUIPrefab;
        }

        public GameObject InstantiateNodeUI(NodeData node)
        {
            GameObject nodeUI = Object.Instantiate(nodeUIPrefab, mapContainer);
            nodeUI.transform.localPosition = CalculatePosition(node.X, node.Y);
            node.UIRepresentation = nodeUI;

            return nodeUI;
        }

        private Vector2 CalculatePosition(int x, int y)
        {
            RectTransform nodeRectTransform = nodeUIPrefab.GetComponent<RectTransform>();
            float horizontalSpacing = 40.0f;
            float verticalSpacing = 60.0f;

            float posX = x * (nodeRectTransform.sizeDelta.x + horizontalSpacing);
            float posY = y * (nodeRectTransform.sizeDelta.y + verticalSpacing);

            return new Vector2(posX, posY);
        }

        public void CreateLine (NodeData startNode, NodeData endNode)
        {
            GameObject lineUI = GameObject.Instantiate(lineUIPrefab, mapContainer);
            lineUI.transform.SetLocalPositionAndRotation(CalculateLinePosition(startNode, endNode), CalculateLineRotation(startNode, endNode));
            lineUI.GetComponent<RectTransform>().sizeDelta = new Vector2(CalculateLineLength(startNode, endNode), 3);
        }

        private Vector2 CalculateLinePosition(NodeData startNode, NodeData endNode)
        {
            Vector2 startPosition = CalculatePosition(startNode.X, startNode.Y);
            Vector2 endPosition = CalculatePosition(endNode.X, endNode.Y);

            return (startPosition + endPosition) / 2;
        }

        private Quaternion CalculateLineRotation(NodeData startNode, NodeData endNode)
        {
            Vector2 startPosition = CalculatePosition(startNode.X, startNode.Y);
            Vector2 endPosition = CalculatePosition(endNode.X, endNode.Y);

            Vector2 direction = endPosition - startPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(0, 0, angle);
        }

        private float CalculateLineLength(NodeData startNode, NodeData endNode)
        {
            Vector2 startPosition = CalculatePosition(startNode.X, startNode.Y);
            Vector2 endPosition = CalculatePosition(endNode.X, endNode.Y);

            return Vector2.Distance(startPosition, endPosition);
        }
    }
}
