using Map;
using NewSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    public class MapGenerator : MonoBehaviour, ISaveable
    {
        public int currentFloor = 0;

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

        public NodeGridGenerator GridGenerator { get => gridGenerator; set => gridGenerator = value; }

        private void Awake()
        {
            SaveManager.RegisterSaveable(this);
        }

        private void Start()
        {
            Initialize();

            //GenerateMap();
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
        public Type GetDataType() => typeof(MapGeneratorSaveData);

        public object Save()
        {
            MapGeneratorSaveData saveData = new()
            {
                currentFloor = currentFloor,
                nodes = gridGenerator.Nodes
            };

            return saveData;
        }

        public void Load(object saveData)
        {
            MapGeneratorSaveData saveDataStruct = (MapGeneratorSaveData)saveData;

            currentFloor = saveDataStruct.currentFloor;
            NodeData[,] nodes = saveDataStruct.nodes;

            Initialize();

            gridGenerator.SetNodes(nodes);
            mapVisualizer.VisualizeMap();
            mapVisualizer.VisualizePaths();
            SetContainerMapHeight();
        }
    }

    public struct MapGeneratorSaveData
    {
        public int currentFloor;
        public NodeData[,] nodes;

        public MapGeneratorSaveData(int currentFloor, NodeData[,] nodes)
        {
            this.currentFloor = currentFloor;
            this.nodes = nodes;
        }
    }
}
