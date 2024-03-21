using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "NodeSpawnCondition", menuName = "Map/NodeSpawnCondition")]
    public class NodeSpawnConditionSO : ScriptableObject
    {
        public NodeType NodeType;
        public int MinFloor;
        public int MaxFloor;
        public float Probability;
    }

    public class NodeRandomizer
    {
        //private List<NodeSpawnConditionSO> nodeSpawnConditions;
        //private int totalFLoors;

        //private NodeType previousNodeType = NodeType.Battle;
        //private NodeType lastTypeOnFloor = NodeType.Battle;

        //public NodeRandomizer(List<NodeSpawnConditionSO> nodeSpawnConditions, int totalFLoors)
        //{
        //    this.nodeSpawnConditions = nodeSpawnConditions;
        //    this.totalFLoors = totalFLoors;
        //}

        //public NodeType GetNodeTypeForFloor(int floor)
        //{
        //    if (floor == 0)
        //    {
        //        return NodeType.Battle;
        //    }
        //    if (floor == totalFLoors / 2)
        //    {
        //        return NodeType.Treasure;
        //    }
        //    if (floor == totalFLoors - 1)
        //    {
        //        return NodeType.Rest;
        //    }
        //    if (floor == totalFLoors - 2)
        //    {
        //        return GetRandomNodeTypeWithExclusions(new List<NodeType> { NodeType.Rest });
        //    }

        //    return GetRandomTypeWithConditions(floor);
        //}

        //private NodeType GetRandomTypeWithConditions(int floor)
        //{
        //    List<NodeSpawnConditionSO> validConditions = new();
        //    foreach (var condition in nodeSpawnConditions)
        //    {
        //        if (floor >= condition.MinFloor && floor <= condition.MaxFloor)
        //        {
        //            validConditions.Add(condition);
        //        }
        //    }

        //    float totalProbability = 0;
        //    foreach (var condition in validConditions)
        //    {
        //        totalProbability += condition.Probability;
        //    }

        //    float randomValue = Random.Range(0, totalProbability);
        //    float cumulativeProbability = 0;

        //    foreach (var condition in validConditions)
        //    {
        //        cumulativeProbability += condition.Probability;
        //        if(randomValue <= cumulativeProbability)
        //        {
        //            NodeType proposedType = condition.NodeType;

        //            if (CanPlaceNodeType(proposedType, floor))
        //            {
        //                lastTypeOnFloor = proposedType;
        //                return proposedType;
        //            }
        //        }
        //    }

        //    return NodeType.Battle;
        //}

        //private NodeType GetRandomNodeTypeWithExclusions(List<NodeType> exclusions)
        //{
        //    List<NodeSpawnConditionSO> validConditions = new();
        //    foreach (var condition in nodeSpawnConditions)
        //    {
        //        if (!exclusions.Contains(condition.NodeType))
        //        {
        //            validConditions.Add(condition);
        //        }
        //    }

        //    return NodeType.Battle;
        //}

        //private bool CanPlaceNodeType(NodeType proposedType, int floor)
        //{
        //    if (floor >= 6 && proposedType == NodeType.Elite && previousNodeType == NodeType.Elite)
        //    {
        //        return false; // No two elites in a row
        //    }

        //    if ((proposedType == NodeType.Rest || proposedType == NodeType.Shop) && lastTypeOnFloor == proposedType)
        //    {
        //        return false; // No two rests or shops in a row
        //    }

        //    return true;
        //}

        private List<NodeSpawnConditionSO> nodeSpawnConditions;

        public NodeRandomizer(List<NodeSpawnConditionSO> nodeSpawnConditions)
        {
            this.nodeSpawnConditions = nodeSpawnConditions;
        }

        public NodeType GetNextNodeType(int floorIndex, List<NodeType> previousFloorNodeTypes)
        {
            var validConditions = GetValidConditions(floorIndex);
            var filteredConditions = validConditions.Where(conditions => !previousFloorNodeTypes.Contains(conditions.NodeType)).ToList();

            if (filteredConditions.Count == 0)
            {
                filteredConditions = validConditions;
            }

            return ChooseRandomType(filteredConditions);
        }

        private List<NodeSpawnConditionSO> GetValidConditions(int floorIndex)
        {
            return nodeSpawnConditions.Where(condition => floorIndex >= condition.MinFloor && floorIndex <= condition.MaxFloor).ToList();
        }

        private NodeType ChooseRandomType(List<NodeSpawnConditionSO> validConditions)
        {
            float totalProbability = validConditions.Sum(condition => condition.Probability);
            float randomValue = Random.Range(0, totalProbability);
            float cumulativeProbability = 0;

            foreach (var condition in validConditions)
            {
                if (randomValue <= condition.Probability)
                {
                    cumulativeProbability += condition.Probability;
                    return condition.NodeType;
                }
            }

            return validConditions.First().NodeType;
        }
    }
}
