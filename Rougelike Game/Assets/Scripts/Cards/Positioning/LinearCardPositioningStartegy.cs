using UnityEngine;

namespace Cards
{
    public class LinearCardPositioningStartegy : ICardPositioningStrategy
    {
        private readonly CardPositioningConfig config;

        public LinearCardPositioningStartegy(CardPositioningConfig config)
        {
            this.config = config;
        }

        public Vector3 CalculateCardPosition(int cardIndex, int totalCards, float cardWidth, float? overrideSpacing, float? overrideScaleMultiplier, int? scaledCardIndex = null)
        {
            float spacing = overrideSpacing ?? config.Spacing;
            float scaleMultiplier = overrideScaleMultiplier ?? config.ScaleMultiplier;

            float totalWidth = (cardWidth + spacing) * totalCards - spacing;
            float startX = -totalWidth / 2 + cardWidth / 2;
            float posX = startX + (cardWidth + spacing) * cardIndex;
            float posY = 0f; //Default Y position;
            float posZ = 0f; //Default Y position;

            if (scaledCardIndex.HasValue)
            {
                float valueToMove = cardWidth * (scaleMultiplier - 1) / 2;
                posX += scaledCardIndex.Value < cardIndex ? valueToMove : 0;
                posX -= scaledCardIndex.Value > cardIndex ? valueToMove : 0;
            }

            return new Vector3 (posX, posY, posZ);
        }
    }
}