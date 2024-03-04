using Managers;
using Sirenix.OdinInspector;
using Spells;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class Card : Draggable, ICardRect
    {
        [InlineEditor]
        [SerializeField] private CardData cardData;
        [SerializeField] private SpellFactory spellFactory;
        [SerializeField] private ICardView cardView;

        private CardAnimator animator;
        private CardPositioner positioner;

        private readonly DragPositionManager positionManager = new();

        public RectTransform RectTransform => rectTransform;

        private void OnValidate()
        {
            cardView ??= GetComponent<ICardView>();

            if (cardData != null)
                cardView.UpdateVisuals(cardData);
        }

        private void Awake() => GetReferences();

        public override void OnBeginDrag(PointerEventData eventData)
        {
            positionManager.RegisterStartPosition(this);
            base.OnBeginDrag(eventData);
        }

        public void Configure(CardData cardData)
        {
            this.cardData = cardData;
            cardView ??= GetComponent<ICardView>();
            gameObject.name = $"Card_{cardData.Spell.SpellName}";
            cardView.UpdateVisuals(cardData);
        }

        public void PlayCard(PlayingCardInfo playingCardInfo)
        {
            if (!CanPlayCard())
                return;

            if (playingCardInfo.DropZone.DropZoneType == DropZoneType.Return)
            {
                ReturnCardToPreviousPosition();
                return;
            }

            if (playingCardInfo.DropZone.DropZoneType == DropZoneType.Unit)
            {
                Spell spellToUse = spellFactory.GetSpellByName(cardData.Spell.SpellName);
                foreach (SpellEffect effect in spellToUse.SpellEffects)
                {
                    effect.ApplyEffect(playingCardInfo.UnitTarget);
                }

                animator.AnimateCardMove(gameObject, playingCardInfo.TransformTarget.position,
                    onComplete: () => SetCardNewParent(this, playingCardInfo.TransformTarget, false));

                positioner.PositionCards();
            }
        }

        public void SetCardNewParent(Card card, Transform newParent, bool showCard = true)
        {
            card.gameObject.transform.SetParent(newParent);
            card.gameObject.SetActive(showCard);
        }

        public void ReturnCardToPreviousPosition() => animator.AnimateCardMove(gameObject, originalPosition, onComplete: OnEndReturningToHand);

        private void OnEndReturningToHand()
        {
            transform.SetParent(ParentToReturnTo);
            positionManager.ResetToStartPosition(this);
        }

        private bool CanPlayCard()
        {
            if (cardData == null)
            {
                Debug.LogWarning("Card data is missing!");
                return false;
            }
            if (cardData.Spell == null)
            {
                Debug.LogWarning("Card spell is missing!");
                return false;
            }

            return true;
        }

        private void GetReferences()
        {
            cardView = GetComponent<ICardView>();
            animator = GetComponent<CardAnimator>();
            positioner = GameManager.Instance.HandCardsPositioner;
        }
    }
}
