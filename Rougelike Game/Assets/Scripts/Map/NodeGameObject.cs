using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    public class NodeGameObject : MonoBehaviour
    {
        private Node node;

        [ShowInInspector] private string id;
        [ShowInInspector] private NodeType type;
        [ShowInInspector] private NodeState state;
        [ShowInInspector] private string[] neighbors;

        public void SetNode(Node node)
        {
            this.node = node;

            id = node.Id;
            type = node.Type;
            state = node.State;

            neighbors = new string[node.Neighbors.Count];
            for (int i = 0; i < node.Neighbors.Count; i++)
            {
                neighbors[i] = node.Neighbors[i].Id;
            }
        }
    }
}
