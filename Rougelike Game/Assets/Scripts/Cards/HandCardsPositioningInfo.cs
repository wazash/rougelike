using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "HandCardsPositioningInfo", menuName = "Cards/HandCardsPositioningInfo")]
    public class HandCardsPositioningInfo : ScriptableObject
    {
        [SerializeField] private float spacing = 10.0f;
        [SerializeField] private float animationDuration = 0.2f;

        public float Spacing => spacing;
        public float AnimationDuration => animationDuration;
    }
}
