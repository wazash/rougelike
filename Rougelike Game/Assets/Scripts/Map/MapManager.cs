using NewSaveSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour, ISaveable
    {
        [Title("Nodes Prefabs")]
        [SerializeField] private List<NodeTypeScriptableObject> nodeTypes;
        [SerializeField] private Node defaultNodePrefab;
        private Dictionary<NodeType, NodeTypeScriptableObject> nodeMapping;

        [Title("Map Screen")]
        [SerializeField] private GameObject mapScreen;
        [SerializeField] private GameObject connectionPrefab;
        [SerializeField] private Transform mapContainer;
        [SerializeField] private Transform connectionsContainer;

        private IMapGeneratorStrategy mapStrategy;
        private List<NodeData> nodes;
        //private Dictionary<string, Vector2> nodePosition = new();
        private Dictionary<string, Node> nodeObjects = new();

        [Title("Map Generation Settings")]
        [InlineEditor]
        [SerializeField] private MapGenerationData mapGenerationData;

        [Title("Debugging")]
        [SerializeField] private bool debugMode = false;

        private HashSet<(string, string)> drawnConnections = new();

        public GameObject MapScreen => mapScreen;

        private void Awake()
        {
            CreateNodeMapping();

            SaveManager.RegisterSaveable(this);
        }

        #region Saving
        public string GetSaveID() => "MapManager";

        public Type GetDataType() => typeof(List<NodeData>);

        public object Save()
        {
            if(nodes == null)
            {
                Debug.LogError("Nodes list is null");
                return null;
            }

            List<NodeData> nodesToSave = new();
            foreach (var node in nodes)
            {
                if (nodeObjects.TryGetValue(node.Id, out Node nodeObject))
                {
                    nodesToSave.Add((NodeData)nodeObject.Save());
                }
            }
            return nodesToSave;
        }

        public void Load(object saveData)
        {
            List<NodeData> loadedNodes = (List<NodeData>)saveData;
            nodes = loadedNodes;

            nodeObjects.Clear();

            ClearMap();
            ClearConnections();
            SetContainerMapHeight();
            DisplayMap();
        }

        #endregion

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
            mapStrategy.CalculateNodePositions(mapContainer as RectTransform);
            SetContainerMapHeight();
            DisplayMap();
        }

        private void SetContainerMapHeight()
        {
            float offset = 150f;
            float mapHeight = 0f;

            NodeData bossNode = nodes.Find(node => node.Type == NodeType.Boss);
            if (!nodeMapping.TryGetValue(NodeType.Boss, out NodeTypeScriptableObject bossTypeSO))
                return;

            mapHeight = bossNode.Position.y + bossTypeSO.NodePrefab.GetComponent<RectTransform>().sizeDelta.y + offset;
            mapContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(mapContainer.GetComponent<RectTransform>().sizeDelta.x, mapHeight);
        }

        private void DisplayMap()
        {
            ClearMap();

            foreach (NodeData node in nodes)
            {
                Node nodeObject = GetNodePrefab(node.Type);

                nodeObject.SetNodeData(node);
                nodeObject.name = $"{node.Id}";
                RectTransform nodeRectTranform = nodeObject.GetComponent<RectTransform>();
                nodeRectTranform.anchoredPosition = nodeObject.Position;

                nodeObjects.Add(node.Id, nodeObject);
            };

            DrawConnections();
        }

        private void ClearMap()
        {
            // Clear the map container
            foreach (Transform child in mapContainer)
            {
                if (child == connectionsContainer)
                    continue;
                Destroy(child.gameObject);
            }
        }

        private Node GetNodePrefab(NodeType nodeType)
        {
            if (nodeMapping.TryGetValue(nodeType, out NodeTypeScriptableObject nodeTypeSO))
            {
                Node nodePrefab = Instantiate(nodeTypeSO.NodePrefab, mapContainer);
                return nodePrefab;
            }

            return Instantiate(nodeTypes[0].NodePrefab, mapContainer);
        }

        private void DrawConnections()
        {
            drawnConnections.Clear();

            foreach (NodeData node in nodes)
            {
                if (nodeObjects.TryGetValue(node.Id, out Node nodeObject) == false)
                {
                    Debug.LogError($"Node {node.Id} does not have a game object");
                    continue;
                }

                foreach (var neighborId in node.NeighborsIds)
                {
                    if (nodeObjects.TryGetValue(neighborId, out Node neighborObject) == false)
                    {
                        Debug.LogError($"Node {neighborId} does not have a game object");
                        continue;
                    }

                    DrawConnection(nodeObject, neighborObject);
                }
            }
        }

        private void DrawConnection(Node startNode, Node endNode)
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

        private void ClearConnections()
        {
            // Clear connections container
            foreach (Transform child in connectionsContainer)
            {
                Destroy(child.gameObject);
            }
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
