using Map;
using System.Collections.Generic;
using UnityEngine;

namespace TestGenerator
{
    public class PathCreator
    {
        private readonly NodeGridGenerator gridGenerator;
        private readonly int width;
        private readonly int height;

        public PathCreator(NodeGridGenerator gridGenerator)
        {
            this.gridGenerator = gridGenerator;
            width = gridGenerator.Width;
            height = gridGenerator.Height;
        }

        public void CreatePath()
        {
            for (int x = 0; x < width - 1; x++)
            {
                CreatePathFrom(x, 0);
            }

            NodeData bossNode = gridGenerator.GetBossNode();
            ConnectPathsToBoss(bossNode);

            RemoveUnconnectedNodes();
        }

        private void CreatePathFrom(int startX, int startY)
        {
            int x = startX;
            int y = startY;

            NodeData currentNode = gridGenerator.Nodes[x, y];

            while (y < height - 1)
            {
                List<int> possibleNextX = GetPossibleNextX(currentNode);

                if (possibleNextX.Count == 0)
                {
                    Debug.LogError("No possible next X");
                    break;
                }

                int nextX = possibleNextX[UnityEngine.Random.Range(0, possibleNextX.Count)];
                int nextY = y + 1;

                NodeData nextNode = gridGenerator.Nodes[nextX, nextY];
                //if (currentNode.Neighbors.Contains(nextNode) || nextNode.Neighbors.Contains(currentNode)) break;

                currentNode.AddNeighbor(nextNode);

                x = nextX;
                y = nextY;
                currentNode = nextNode;
            }
        }

        private List<int> GetPossibleNextX(NodeData current)
        {
            List<int> possibleNextX = new() { current.X - 1, current.X, current.X + 1 }; // Initial all possible next X

            if (current.X == 0) possibleNextX.Remove(current.X - 1); // Remove left node if current node is at the left edge
            if (current.X == width - 1) possibleNextX.Remove(current.X + 1); // Remove right node if current node is at the right edge

            NodeData leftNode = current.X > 0 ? gridGenerator.Nodes[current.X - 1, current.Y] : null;
            NodeData rightNode = current.X < width - 1 ? gridGenerator.Nodes[current.X + 1, current.Y] : null;
            NodeData upNode = current.Y < height - 1 ? gridGenerator.Nodes[current.X, current.Y + 1] : null;

            if (leftNode != null && leftNode.Neighbors.Contains(upNode)) possibleNextX.Remove(current.X - 1); // Remove up left node if it's connected to up node
            if (rightNode != null && rightNode.Neighbors.Contains(upNode)) possibleNextX.Remove(current.X + 1);// Remove up right node if it's connected to up node

            return possibleNextX;
        }

        private void ConnectPathsToBoss(NodeData bossNode)
        {
            for (int x = 0; x < width; x++)
            {
                NodeData node = gridGenerator.Nodes[x, height - 1];

                if (node != null && node.Neighbors.Count > 0)
                    node.AddNeighbor(bossNode);
            }
        }

        private void RemoveUnconnectedNodes()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    NodeData node = gridGenerator.Nodes[x, y];
                    if (node.Neighbors.Count == 0)
                    {
                        gridGenerator.Nodes[x, y] = null;
                    }
                }
            }
        }
    }
}
