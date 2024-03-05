using UnityEngine;

namespace Cards
{
    public interface ICardPositioningStrategy
    {
        Vector3 CalculateCardPosition(int cardIndex, int totalCards, float cardWidth, float? overrideSpacing, float? overrideScaleMultiplier, int? scaledCardIndex = null);
    }
}