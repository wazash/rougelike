using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardPositioningConfig", menuName = "Cards/CardPositioningConfig")]
    public class CardPositioningConfig : ScriptableObject
    {
        [SerializeField] private float spacing = 10.0f;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float scaleMultiplier = 1.2f;

        public float Spacing => spacing;
        public float AnimationDuration => animationDuration;
        public float ScaleMultiplier => scaleMultiplier;
    }
}
