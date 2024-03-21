﻿using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class HorizontalMapStrategy : IMapGeneratorStrategy
    {
        private int maxFloors;
        private int maxBranches;
        private MinMaxInt nodesOnFloor;

        public HorizontalMapStrategy(int maxFloors, int maxBranches, MinMaxInt nodesOnFloor)
        {
            this.maxFloors = maxFloors;
            this.maxBranches = maxBranches;
            this.nodesOnFloor = nodesOnFloor;
        }

        public void CalculateNodePositions(RectTransform mapContainer)
        {
            throw new System.NotImplementedException();
        }

        public List<NodeData> GenerateMap(int startPathsCount, float branchingProbability)
        {
            // Horizontal map generation logic
            return new List<NodeData>();
        }
    }
}