using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class RadialMapStrategy : IMapGeneratorStrategy
    {
        public void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer)
        {
            throw new System.NotImplementedException();
        }

        public List<Node> GenerateMap(int startPathsCount, float branchingProbability)
        {
            // Radial map generation logic
            return new List<Node>();
        }
    }
}
