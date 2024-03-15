using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "MapGenerationData", menuName = "Map/MapGenerationData")]
    public class MapGenerationData : ScriptableObject
    {
        [SerializeField, Range(3, 5)] private int startPathsCount = 3;
        [SerializeField, Range(0f, 1f)] private float branchingProbability = 0.1f;
        [SerializeField, Range(2, 30)] private int maxFloors = 10;
        [SerializeField, Range(1, 4)] private int maxBranches = 3;
        [SerializeField] private MinMaxInt nodesOnFloor = new(3, 5);

        public int StartPathsCount => startPathsCount;
        public float BranchingProbability => branchingProbability;
        public int MaxFloors => maxFloors;
        public int MaxBranches => maxBranches;
        public MinMaxInt NodesOnFloor => nodesOnFloor;
    }
}
