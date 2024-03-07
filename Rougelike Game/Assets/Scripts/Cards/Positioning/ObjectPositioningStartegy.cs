using UnityEngine;

namespace Cards
{
    public class ObjectPositioningStartegy : IObjectPositionerStrategy
    {
        private readonly ObjectPositioningConfig config;

        public ObjectPositioningStartegy(ObjectPositioningConfig config)
        {
            this.config = config;
        }

        public Vector3 CalculateObjectPosition(
            int objectIndex,
            int totalObjects,
            float objectSize,
            PositioningType? overridePositioningType,
            float? overrideSpacing,
            float? overrideScaleMultiplier,
            int? scaledObjectIndex)
        {
            float spacing = overrideSpacing ?? config.Spacing;
            float scaleMultiplier = overrideScaleMultiplier ?? config.ScaleMultiplier;
            PositioningType positioningType = overridePositioningType ?? config.PositioningType;

            float posX, posY, posZ;

            float totalSize = (objectSize + spacing) * totalObjects - spacing;
            float startPos = -totalSize / 2 + objectSize / 2;


            if (positioningType == PositioningType.Horizontal)
            {
                posX = startPos + (objectSize + spacing) * objectIndex;
                posY = 0f;
                posZ = 0f;

                AdjustForScaling(ref posX, objectIndex, objectSize, scaleMultiplier, scaledObjectIndex);
            }
            else
            {
                posX = 0f;
                posY = startPos + (objectSize + spacing) * objectIndex;
                posZ = 0f;

                AdjustForScaling(ref posY, objectIndex, objectSize, scaleMultiplier, scaledObjectIndex);
            }

            return new Vector3 (posX, posY, posZ);
        }

        private void AdjustForScaling(ref float position, int objectIndex, float objectSize, float scaleMultiplier, int? scaledObjectIndex)
        {
            if (scaledObjectIndex.HasValue)
            {
                float valueToMove = objectSize * (scaleMultiplier - 1) / 2;
                position += scaledObjectIndex.Value < objectIndex ? valueToMove : 0;
                position -= scaledObjectIndex.Value > objectIndex ? valueToMove : 0;
            }
        }
    }
}