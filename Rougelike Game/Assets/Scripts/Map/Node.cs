using SaveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Node : MonoBehaviour, IBind<NodeData>
    {
        [SerializeField] private NodeData data;
        [field: SerializeField] public string Id { get; private set; }
        [SerializeField] private Vector2 position;
        [SerializeField] private NodeType type;
        [SerializeField] private NodeState state;
        [SerializeField] private List<string> neighbors;


        //[ShowInInspector] private string id;
        //[ShowInInspector] private NodeType type;
        //[ShowInInspector] private NodeState state;
        //[ShowInInspector] private string[] neighbors;

        public void Bind(NodeData data)
        {
            this.data = data;
            this.data.Id = Id;

            position = data.Position;
        }

        public void SetNodeData(NodeData data)
        {
            Id = data.Id;
            position = data.Position;
            type = data.Type;
            state = data.State;
            foreach (var neighbor in data.Neighbors)
            {
                neighbors.Add(neighbor.Id);
            }
        }
    }
}
