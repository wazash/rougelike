using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "HorizontalObjectPositioningConfig", menuName = "Horizontal Positioning/PositioningConfig")]
    public class ObjectPositioningConfig : ScriptableObject
    {
        [SerializeField] private float spacing = 10.0f;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float scaleMultiplier = 1.2f;
        [SerializeField] private PositioningType positioningType = PositioningType.Horizontal;

        public float Spacing => spacing;
        public float AnimationDuration => animationDuration;
        public float ScaleMultiplier => scaleMultiplier;
        public PositioningType PositioningType => positioningType;
    }
}
