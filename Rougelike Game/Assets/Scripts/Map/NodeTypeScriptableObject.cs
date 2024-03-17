using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "NodeType", menuName = "Map/NodeType")]
    public class NodeTypeScriptableObject : ScriptableObject
    {
        [field: SerializeField] public NodeType NodeType { get; set; }
        [field: SerializeField] public Node NodePrefab { get; set; }
    }
}
