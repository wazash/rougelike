﻿using NewSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Node : MonoBehaviour, ISaveable
    {
        private NodeData data;
        [field: SerializeField] public string Id { get; private set; }
        private int x, y;
        [SerializeField] private Vector2 position;
        [SerializeField] private NodeType type;
        [SerializeField] private NodeState state;
        [SerializeField] private List<string> neighborsId;

        public Vector2 Position => position;

        public void SetNodeData(NodeData data)
        {
            Id = data.Id;
            x = data.X;
            y = data.Y;
            position = data.Position;
            type = data.Type;
            state = data.State;
            neighborsId = new();
            foreach (var neighborId in data.NeighborsIds)
            {
                neighborsId.Add(neighborId);
            }
        }

        public string GetSaveID() => Id;
        public Type GetDataType() => typeof(NodeData);

        public object Save()
        {
            return new NodeData()
            {
                Id = Id,
                X = x,
                Y = y,
                Position = position,
                Type = type,
                State = state,
                NeighborsIds = neighborsId
            };
        }

        public void Load(object saveData)
        {
            NodeData nodeData = (NodeData)saveData;
            SetNodeData(nodeData);
        }

    }
}
