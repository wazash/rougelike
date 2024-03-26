using Map;
using NewSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    public class MapGenerator : MonoBehaviour, ISaveable
    {
        public List<NodeTypeScriptableObject> nodeUIPrefabs;
        public GameObject pathUIPrefab;
        public Transform mapContainer;
        public Transform pathsContainer;

        public int width = 7;
        public int height = 15;

        public GameObject mapScreen;

        private NodeGridGenerator gridGenerator;
        private PathCreator pathCreator;
        private MapVisualizer mapVisualizer;
        private NodeTypeAssigner nodeTypeAssigner;

        private void Awake()
        {
            SaveManager.RegisterSaveable(this);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            gridGenerator ??= new(width, height);
            pathCreator ??= new(gridGenerator);
            mapVisualizer ??= new(gridGenerator, nodeUIPrefabs, pathUIPrefab, mapContainer, pathsContainer);
            nodeTypeAssigner ??= new(gridGenerator);
        }

        public void GenerateMap()
        {
            gridGenerator.GenerateGrid();

            pathCreator.CreatePath();
            nodeTypeAssigner.AssingNodeTypes();

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

        public string GetSaveID() => "MapGenerator";
        public Type GetDataType() => typeof(NodeData[,]);

        public object Save() => gridGenerator.Nodes;

        public void Load(object saveData)
        {
            NodeData[,] nodes = (NodeData[,])saveData;

            Initialize();

            gridGenerator.SetNodes(nodes);
            mapVisualizer.VisualizeMap();
            mapVisualizer.VisualizePaths();
            SetContainerMapHeight();
        }
    }
}
