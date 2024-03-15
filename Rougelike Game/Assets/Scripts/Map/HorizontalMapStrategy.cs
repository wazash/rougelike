using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class HorizontalMapStrategy : IMapGeneratorStrategy
    {
        public void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer)
        {
            throw new System.NotImplementedException();
        }

        public List<Node> GenerateMap(int startPathsCount, float branchingProbability)
        {
            // Horizontal map generation logic
            return new List<Node>();
        }
    }
}
