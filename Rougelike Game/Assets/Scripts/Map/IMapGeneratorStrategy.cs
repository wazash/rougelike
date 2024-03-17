using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public interface IMapGeneratorStrategy
    {
        List<NodeData> GenerateMap(int startPathsCount, float branchingProbability);
        void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer);
    }
}
