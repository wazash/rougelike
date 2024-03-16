using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        [Title("Nodes Prefabs")]
        [SerializeField] private List<NodeTypeScriptableObject> nodeTypes;
        [SerializeField] private GameObject defaultNodePrefab;
        private Dictionary<NodeType, NodeTypeScriptableObject> nodeMapping;

        [Title("Map Screen")]
        [SerializeField] private GameObject mapScreen;
        [SerializeField] private GameObject connectionPrefab;
        [SerializeField] private Transform mapContainer;
        [SerializeField] private Transform connectionsContainer;

        private IMapGeneratorStrategy mapStrategy;
        private List<Node> nodes;
        private Dictionary<string, Vector2> nodePosition = new();
        private Dictionary<string, GameObject> nodeObjects = new();

        [Title("Map Generation Settings")]
        [InlineEditor]
        [SerializeField] private MapGenerationData mapGenerationData;

        [Title("Debugging")]
        [SerializeField] private bool debugMode = false;

        private HashSet<(string, string)> drawnConnections = new();

        public GameObject MapScreen => mapScreen;

        private void Awake() => CreateNodeMapping();

        private void Start()
        {
            if (debugMode)
            {
                DebugGenerateMap();
            }
        }

        private void DebugGenerateMap()
        {
            CreateNodeMapping();
            mapStrategy = new VerticalMapStrategy(mapGenerationData.MaxFloors, mapGenerationData.MaxBranches, mapGenerationData.NodesOnFloor);
            GenerateMap(mapStrategy, mapGenerationData);
        }

        public void SetMapGenerationStartegy(IMapGeneratorStrategy strategy) => mapStrategy = strategy;

        public void GenerateMap(IMapGeneratorStrategy mapStrategy, MapGenerationData mapData)
        {
            nodes = mapStrategy.GenerateMap(mapData.StartPathsCount, mapData.BranchingProbability);
            mapStrategy.CalculateNodePositions(nodePosition, mapContainer as RectTransform);
            SetContainerMapHeight();
            DisplayMap();
        }

        private void SetContainerMapHeight()
        {
            float offset = 150f;
            float mapHeight = 0f;

            Node bossNode = nodes.Find(node => node.Type == NodeType.Boss);
            if (!nodeMapping.TryGetValue(NodeType.Boss, out NodeTypeScriptableObject bossTypeSO))
                return;

            if (!nodePosition.TryGetValue(bossNode.Id, out Vector2 bossPosition))
                return;

            mapHeight = bossPosition.y + bossTypeSO.NodePrefab.GetComponent<RectTransform>().sizeDelta.y + offset;
            mapContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(mapContainer.GetComponent<RectTransform>().sizeDelta.x, mapHeight);
        }

        private void DisplayMap()
        {
            // Clear the map container
            foreach (Transform child in mapContainer)
            {
                if (child == connectionsContainer)
                    continue;
                Destroy(child.gameObject);
            }

            foreach (Node node in nodes)
            {
                if (nodePosition.TryGetValue(node.Id, out Vector2 position) == false)
                {
                    Debug.LogError($"Node {node.Id} does not have a position");
                    continue;
                }

                GameObject nodeObject = GetNodePrefab(node.Type);

                nodeObject.name = $"{node.Id}";
                nodeObject.GetComponent<NodeGameObject>().SetNode(node);
                RectTransform nodeRectTranform = nodeObject.GetComponent<RectTransform>();
                nodeRectTranform.anchoredPosition = position;
                nodeObjects[node.Id] = nodeObject;
            }

            DrawConnections();
        }

        private GameObject GetNodePrefab(NodeType nodeType)
        {
            if (nodeMapping.TryGetValue(nodeType, out NodeTypeScriptableObject nodeTypeSO))
            {
                GameObject nodePrefab = Instantiate(nodeTypeSO.NodePrefab, mapContainer);
                return nodePrefab;
            }

            return Instantiate(nodeTypes[0].NodePrefab, mapContainer);
        }

        private void DrawConnections()
        {
            foreach (Node node in nodes)
            {
                if (nodeObjects.TryGetValue(node.Id, out GameObject nodeObject) == false)
                {
                    Debug.LogError($"Node {node.Id} does not have a game object");
                    continue;
                }

                foreach (Node neighbor in node.Neighbors)
                {
                    if (nodeObjects.TryGetValue(neighbor.Id, out GameObject neighborObject) == false)
                    {
                        Debug.LogError($"Node {neighbor.Id} does not have a game object");
                        continue;
                    }

                    DrawConnection(nodeObject, neighborObject);
                }
            }
        }

        private void DrawConnection(GameObject startNode, GameObject endNode)
        {
            string startNodeId = startNode.name;
            string endNodeId = endNode.name;

            // Check if the connection has already been drawn, if so, return
            if (drawnConnections.Contains((startNodeId, endNodeId)) || drawnConnections.Contains((endNodeId, startNodeId)))
                return;

            RectTransform connection = Instantiate(connectionPrefab, connectionsContainer).GetComponent<RectTransform>();
            connection.name = $"Connection_{startNode.name}-{endNode.name}";

            Vector2 startPosition = startNode.GetComponent<RectTransform>().anchoredPosition;
            Vector2 endPosition = endNode.GetComponent<RectTransform>().anchoredPosition;
            Vector2 direction = endPosition - startPosition;
            float distance = direction.magnitude;

            connection.anchoredPosition = startPosition + direction / 2;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            connection.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            connection.sizeDelta = new Vector2(connection.sizeDelta.x, distance);

            drawnConnections.Add((startNodeId, endNodeId));
        }

        private void CreateNodeMapping()
        {
            nodeMapping = new Dictionary<NodeType, NodeTypeScriptableObject>();
            foreach (NodeTypeScriptableObject nodeType in nodeTypes)
            {
                nodeMapping[nodeType.NodeType] = nodeType;
            }
        }
    }
}
