using Map;

namespace MapGenerator
{
    public class NodeGridGenerator 
    {
        public int Width { get; }
        public int Height { get; }
        public NodeData[,] Nodes { get; private set; }

        public NodeGridGenerator(int width, int height)
        {
            Width = width;
            Height = height;
            Nodes = new NodeData[Width, Height + 1];
        }

        public void GenerateGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Nodes[x, y] = new NodeData(x, y)
                    {
                        Id = $"node_{x}_{y}"
                    };
                }
            }

            GenerateBossNode();
        }

        public void SetNodes(NodeData[,] nodes) => Nodes = nodes;

        public NodeData GenerateBossNode()
        {
            NodeData bossNode = new(Width / 2, Height);
            Nodes[Width / 2, Height] = bossNode;
            bossNode.Id = "node_boss";
            return bossNode;
        }

        public NodeData GetBossNode() => Nodes[Width / 2, Height];

        public NodeData GetNodeById(string neighborId)
        {
            foreach (var node in Nodes)
            {
                if (node == null) continue;

                if (node.Id == neighborId)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
