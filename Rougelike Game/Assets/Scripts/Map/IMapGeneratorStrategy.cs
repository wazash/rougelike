using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Map
{
    public interface IMapGeneratorStrategy
    {
        List<Node> GenerateMap(int numerOdNodes, int startPathsCount, float branchingProbability);
        void CalculateNodePositions(Dictionary<string, Vector2> nodePositions, RectTransform mapContainer);
    }
}
