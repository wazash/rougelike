using UnityEngine;

namespace Cards
{
    public enum PositioningType { Horizontal, Vertical }

    public interface IObjectPositionerStrategy
    {
        /// <summary>
        /// Calculates a new position for an RectTransform object based on the number of objects in the parent
        /// </summary>
        /// <param name="objectIndex">Current object index</param>
        /// <param name="totalObjets">Total objects in parent</param>
        /// <param name="objectSize">Size of individual object, width for horizontal, height for vertical</param>
        /// <param name="overridePositioningType">Positioning type, horizontal or vertical.</param>
        /// <param name="overrideSpacing">Space between objetcs</param>
        /// <param name="overrideScaleMultiplier">Scaling multiplier, needed for object that is scaled (for example hovered)</param>
        /// <param name="scaledObjectIndex">Scaled object index, needed for object that is scaled (for example hovered)</param>
        /// <returns></returns>
        Vector3 CalculateObjectPosition(
            int objectIndex,
            int totalObjets,
            float objectSize,
            PositioningType? overridePositioningType,
            float? overrideSpacing,
            float? overrideScaleMultiplier,
            int? scaledObjectIndex = null);
    }
}