using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class NodeVisual : MonoBehaviour
    {
        [SerializeField] private Button button;

        public void UpdateVisual(Node node)
        {
            button.interactable = node.State == NodeState.Unlocked;
        }
    }
}
