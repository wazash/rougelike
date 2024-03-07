using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class ObjectPositioner
    {
        private readonly GameObject handGameObject;
        private readonly IObjectPositionerStrategy positioningStrategy;
        private ObjectPositioningConfig positioningConfig;

        /// <summary>
        /// Creating instance of objet positioner.
        /// </summary>
        /// <param name="handGameObject">Parent object for positionable objects</param>
        /// <param name="positioningConfig">ObjectPositioningConfig SriptableObject with configuration data</param>
        public ObjectPositioner(GameObject handGameObject, ObjectPositioningConfig positioningConfig)
        {
            this.handGameObject = handGameObject;
            this.positioningConfig = positioningConfig;
            this.positioningStrategy = new ObjectPositioningStartegy(positioningConfig);
        }

        public void SetPositioningConfig(ObjectPositioningConfig newConfig)
        {
            this.positioningConfig = newConfig;
        }

        /// <summary>
        /// Position object. Positioner take IPositionableObject size depends on what positioning type is set.
        /// </summary>
        /// <param name="positionableObjet">Positionable object to position</param>
        /// <param name="positioningType">Positioning type: base on configuration SO</param>
        /// <param name="scaledObjectIndex">Object index to scale</param>
        /// <param name="initialSetup">If true: set position instantly, if false: set position smoothly</param>
        public void PositionObjects(PositioningType? positioningType = null, int? scaledObjectIndex = null, bool initialSetup = false)
        {
            var objects = handGameObject.GetComponentsInChildren<IPositionableObject>().Select(@object => @object.RectTransform).ToArray();
            int objectsCount = objects.Length;
            PositioningType type = positioningType ?? positioningConfig.PositioningType;
            float objectSize = 0;
            IPositionableObject[] posObjects = handGameObject.GetComponentsInChildren<IPositionableObject>();

            if (objectsCount <= 0)
            {
                Debug.LogWarning($"Object are missing in hand gameobject.");
                return;
            }
            if (positioningConfig == null)
            {
                Debug.LogWarning("Positioning Config is missing. Check if you properly assing it when creating instance.");
                return;
            }

            for (int i = 0; i < objectsCount; i++)
            {
                if (type == PositioningType.Horizontal)
                {
                    objectSize = posObjects[i].OriginalSizeDelta.x;
                }
                if (type == PositioningType.Vertical)
                {
                    objectSize = posObjects[i].OriginalSizeDelta.y;
                }

                Vector3 targetPosition = positioningStrategy
                    .CalculateObjectPosition(
                        objectIndex: i,
                        totalObjets: objectsCount,
                        objectSize: objectSize,
                        overridePositioningType: positioningType,
                        overrideSpacing: positioningConfig.Spacing,
                        overrideScaleMultiplier: positioningConfig.ScaleMultiplier,
                        scaledObjectIndex: scaledObjectIndex);

                if (initialSetup)
                    objects[i].localPosition = targetPosition;
                else
                    objects[i].DOLocalMove(targetPosition, positioningConfig.AnimationDuration).SetEase(Ease.OutQuad);
            }
        }
    }
}