using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardPositioner
    {
        private readonly GameObject handGameObject;
        private ICardPositioningStrategy positioningStrategy;
        private readonly CardPositioningConfig positioningConfig;

        public CardPositioner(GameObject handGameObject, CardPositioningConfig positioningConfig)
        {
            this.handGameObject = handGameObject;
            this.positioningConfig = positioningConfig;
            this.positioningStrategy = new LinearCardPositioningStartegy(positioningConfig);
        }

        public void SetPositioningStrategy(ICardPositioningStrategy newStrategy)
        {
            this.positioningStrategy = newStrategy;
        }

        public void PositionCards(float width, int? scaledCardIndex = null, bool initialSetup = false)
        {
            var cards = handGameObject.GetComponentsInChildren<ICardRect>().Select(card => card.RectTransform).ToArray();
            int cardCount = cards.Length;

            if (cardCount <= 0)
            {
                Debug.LogWarning($"Cards are missing in hand gameobject.");
                return;
            }

            if (positioningConfig == null)
            {
                Debug.LogWarning("Positioning Config is missing. Check if you properly assing it when creating instance.");
                return;
            }

            for (int i = 0; i < cardCount; i++)
            {
                Vector3 targetPosition = positioningStrategy
                    .CalculateCardPosition(i, cardCount, width, positioningConfig.Spacing, positioningConfig.ScaleMultiplier, scaledCardIndex);

                if (initialSetup)
                {
                    cards[i].localPosition = targetPosition;
                }
                else
                {
                    cards[i].DOLocalMove(targetPosition, positioningConfig.AnimationDuration).SetEase(Ease.OutQuad);
                }
            }
        }
    }
}