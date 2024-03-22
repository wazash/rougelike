using Map;

namespace TestGenerator
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
        }

        public void GenerateGrid()
        {
            Nodes = new NodeData[Width, Height + 1];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Nodes[x, y] = new NodeData(x, y)
                    {
                        Id = $"node_{y}_{x}"
                    };
                }
            }

            GenerateBossNode();
        }

        public NodeData GenerateBossNode()
        {
            NodeData bossNode = new(Width / 2, Height);
            Nodes[Width / 2, Height] = bossNode;
            bossNode.Id = "node_boss";
            return bossNode;
        }

        public NodeData GetBossNode() => Nodes[Width / 2, Height];
    }
}
