using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapController : MonoBehaviour
    {
        public GameObject nodePrefab;
        public GameObject connectionPrefab;
        public Transform mapContainer;
        public Transform connectionsContainer;

        private IMapGeneratorStrategy mapStrategy;
        private List<Node> nodes;
        private Dictionary<string, Vector2> nodePosition = new();
        private Dictionary<string, GameObject> nodeObjects = new();

        [Title("Map Generation Settings")]
        [InlineEditor]
        [SerializeField] private MapGenerationData mapGenerationData;

        private HashSet<(string, string)> drawnConnections = new();

        private void Start()
        {
            mapStrategy = new VerticalMapStrategy(mapGenerationData.MaxFloors, mapGenerationData.MaxBranches, 
                mapGenerationData.NodesOnFloor.MinValue, mapGenerationData.NodesOnFloor.MaxValue);
            nodes = mapStrategy.GenerateMap(mapGenerationData.StartPathsCount, mapGenerationData.BranchingProbability);

            mapStrategy.CalculateNodePositions(nodePosition, mapContainer as RectTransform);
            DisplayMap();
        }

        private void DisplayMap()
        {
            foreach (Node node in nodes)
            {
                if (nodePosition.TryGetValue(node.Id, out Vector2 position) == false)
                {
                    Debug.LogError($"Node {node.Id} does not have a position");
                    continue;
                }

                GameObject nodeObject = Instantiate(nodePrefab, mapContainer);
                nodeObject.name = $"{node.Id}";
                RectTransform nodeRectTranform = nodeObject.GetComponent<RectTransform>();
                nodeRectTranform.anchoredPosition = position;
                nodeObjects[node.Id] = nodeObject;
            }

            DrawConnections();
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

            if(drawnConnections.Contains((startNodeId, endNodeId)) || drawnConnections.Contains((endNodeId, startNodeId)))
            {
                return;
            }

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
    }
}
