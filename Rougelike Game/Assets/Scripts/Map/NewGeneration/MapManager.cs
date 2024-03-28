using Map;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MapGenerator
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;

        private NodeData[,] nodeGridGenerators;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            nodeGridGenerators = mapGenerator.GridGenerator.Nodes;

            //RegisterNodesActions();
        }

        public void RegisterNodesActions()
        {
            for (int x = 0; x < mapGenerator.width; x++)
            {
                for (int y = 0; y < mapGenerator.height; y++)
                {
                    NodeData node = nodeGridGenerators[x, y];
                    if (node == null) continue;

                    node.UIRepresentation.GetComponent<Button>().onClick.RemoveAllListeners();
                    node.UIRepresentation.GetComponent<Button>().onClick.AddListener(() => node.UIRepresentation.NodeRunner());
                }
            }

            NodeData bossNode = mapGenerator.GridGenerator.GetBossNode();
            bossNode.UIRepresentation.GetComponent<Button>().onClick.RemoveAllListeners();
            bossNode.UIRepresentation.GetComponent<Button>().onClick.AddListener(() => bossNode.UIRepresentation.NodeRunner());
        }

    }
}
