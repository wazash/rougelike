using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardPositioner
    {
        private readonly GameObject handGameObject;
        private readonly HandCardsPositioningInfo positioningInfo;

        private float spacing, animationDuration;

        public CardPositioner(GameObject handGameObject, HandCardsPositioningInfo handCardsPositioningInfo)
        {
            this.handGameObject = handGameObject;
            this.positioningInfo = handCardsPositioningInfo;
        }

        public CardPositioner(GameObject handGameObject)
        {
            this.handGameObject = handGameObject;
        }

        public void PositionCards(bool initialSetup = false, float spacing = 10f, float animationDuration = 0.2f)
        {
            var cards = handGameObject.GetComponentsInChildren<ICardRect>().Select(card => card.RectTransform).ToArray();
            int cardCount = cards.Length;

            if (cardCount <= 0)
            {
                Debug.LogWarning($"Cards are missing in hand gameobject.");
                return;
            }

            float cardWidth = cards[0].sizeDelta.x;

            if (positioningInfo == null)
            {
                this.spacing = spacing;
                this.animationDuration = animationDuration;
            }
            else
            {
                this.spacing = positioningInfo.Spacing;
                this.animationDuration = animationDuration;
            }

            float totalWidth = (cardWidth + this.spacing) * cardCount - this.spacing;
            float startX = -totalWidth / 2 + cardWidth / 2;

            for (int i = 0; i < cardCount; i++)
            {
                Transform cardTransform = cards[i].transform;
                float posX = startX + (cardWidth + this.spacing) * i;
                Vector3 targetPosition = new(posX, cardTransform.localPosition.y, cardTransform.localPosition.z);

                if (initialSetup)
                {
                    cardTransform.localPosition = targetPosition;
                }
                else
                {
                    cardTransform.DOLocalMove(targetPosition, this.animationDuration).SetEase(Ease.OutQuad);
                }
            }
        }
    }
}