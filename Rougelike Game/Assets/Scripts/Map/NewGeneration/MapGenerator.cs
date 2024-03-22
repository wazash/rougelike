using Map;
using UnityEngine;

namespace TestGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        public Node nodeUIPrefab;
        public GameObject pathUIPrefab;
        public Transform mapContainer;
        public Transform pathsContainer;
        public NodeData[,] nodes;

        public int width = 7;
        public int height = 15;

        private NodeGridGenerator gridGenerator;
        private PathCreator pathCreator;
        private MapVisualizer mapVisualizer;

        private void Start()
        {
            gridGenerator = new(width, height);
            gridGenerator.GenerateGrid();

            pathCreator = new(gridGenerator);
            pathCreator.CreatePath();

            mapVisualizer = new(gridGenerator, nodeUIPrefab, pathUIPrefab, mapContainer, pathsContainer);
            mapVisualizer.VisualizeMap();
            mapVisualizer.VisualizePaths();

            SetContainerMapHeight();
        }

        private void SetContainerMapHeight()
        {
            float offset = 150f;
            NodeData bossNode = gridGenerator.GetBossNode();
            RectTransform bossRect = bossNode.UIRepresentation.GetComponent<RectTransform>();

            float mapHeight = bossRect.localPosition.y + bossRect.sizeDelta.y + offset;
            mapContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(mapContainer.GetComponent<RectTransform>().sizeDelta.x, mapHeight);
        }
    }
}
