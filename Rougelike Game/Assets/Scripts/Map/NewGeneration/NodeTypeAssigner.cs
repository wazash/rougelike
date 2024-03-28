using Map;
using System.Collections.Generic;

namespace MapGenerator
{
    public class NodeTypeAssigner
    {
        private NodeGridGenerator gridGenerator;

        private NodeType[] beforeSixTypes = new NodeType[]
        {
            NodeType.Battle,
            NodeType.Event,
            NodeType.Shop,
            NodeType.Treasure
        };

        public NodeTypeAssigner(NodeGridGenerator gridGenerator)
        {
            this.gridGenerator = gridGenerator;
        }

        public void AssingNodeTypes()
        {

            for (int x = 0; x < gridGenerator.Width; x++)
            {
                for (int y = 0; y < gridGenerator.Height; y++)
                {
                    NodeData node = gridGenerator.Nodes[x, y];
                    if (node == null || node.Type != NodeType.Empty) continue;

                    AssingNodeType(node, x, y);
                }
            }
            AssingBossAndSpecialFloorTypes();
        }

        private void AssingNodeType(NodeData node, int x, int y)
        {
            if (node.Id == "node_boss" || y == gridGenerator.Height) return;

            if (y < 6)
            {
                node.Type = GetBeforSixType();
            }
            else
            {
                node.Type = GetRandomType(node, x, y);
            }
        }

        private NodeType GetBeforSixType()
        {
            return beforeSixTypes[UnityEngine.Random.Range(0, beforeSixTypes.Length)];
        }

        private NodeType GetRandomType(NodeData node, int x, int y)
        {
            List<NodeType> possibleTypes = new() { NodeType.Battle, NodeType.Event, NodeType.Shop, NodeType.Treasure, NodeType.Rest, NodeType.Elite };
            List<NodeType> dontRepeatTypes = new() { NodeType.Shop, NodeType.Rest, NodeType.Elite };
            List<NodeType> previousNeighborsTypes = GetLowerNeighborsTypes(node);

            foreach (NodeType type in previousNeighborsTypes)
            {
                if (dontRepeatTypes.Contains(type))
                {
                    possibleTypes.Remove(type);
                }
            }

            return possibleTypes[UnityEngine.Random.Range(0, possibleTypes.Count)];
        }

        private NodeType GenerateRandomWithExclusion(List<NodeType> possibleTypes, List<NodeType> excludeTypes)
        {
            List<NodeType> types = new List<NodeType>(possibleTypes);
            foreach (NodeType type in excludeTypes)
            {
                types.Remove(type);
            }

            return types[UnityEngine.Random.Range(0, types.Count)];
        }

        private void AssingBossAndSpecialFloorTypes()
        {
            for (int x = 0; x < gridGenerator.Width; x++)
            {
                AssingFirstFloor(x);
                Assing6thFloor(x);
                AssingSecondToLastFloor(x);
                AssingLastFloor(x);
            }

            NodeData bossNode = gridGenerator.GetBossNode();
            if (bossNode != null) bossNode.Type = NodeType.Boss;
        }

        private void AssingFirstFloor(int x)
        {
            NodeData node = gridGenerator.Nodes[x, 0];
            if (node != null) node.Type = NodeType.Battle;
        }
        private void Assing6thFloor(int x)
        {
            NodeData node = gridGenerator.Nodes[x, 6];
            if (node != null) node.Type = NodeType.Treasure;
        }
        private void AssingSecondToLastFloor(int x)
        {
            NodeData node = gridGenerator.Nodes[x, gridGenerator.Height - 2];
            if (node != null)
            {
                List<NodeType> possibleTypes = new() { NodeType.Battle, NodeType.Event, NodeType.Shop, NodeType.Treasure, NodeType.Rest, NodeType.Elite };
                List<NodeType> excludeTypes = new() { NodeType.Rest };
                node.Type = GenerateRandomWithExclusion(possibleTypes, excludeTypes);
            }
        }
        private void AssingLastFloor(int x)
        {
            NodeData node = gridGenerator.Nodes[x, gridGenerator.Height - 1];
            if (node != null) node.Type = NodeType.Rest;
        }

        private List<NodeType> GetLowerNeighborsTypes(NodeData node)
        {
            List<NodeType> lowerNeighborsTypes = new();
            foreach (string neighborId in node.NeighborsIds)
            {
                NodeData neighbor = gridGenerator.GetNodeById(neighborId);
                if (neighbor != null && neighbor.Y < node.Y)
                {
                    lowerNeighborsTypes.Add(neighbor.Type);
                }
            }

            return lowerNeighborsTypes;
        }
    }
}
