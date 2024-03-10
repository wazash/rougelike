using Managers;
using Sirenix.OdinInspector;
using Spells;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class Card : Draggable, IPositionableObject
    {
        [InlineEditor, SerializeField] private CardData cardData;
        [SerializeField] private SpellFactory spellFactory;
        [SerializeField] private ICardView cardView;

        private CardAnimator animator;
        private GameManager gameManager;
        private ObjectPositioner positioner;
        private readonly DragPositionManager positionManager = new();

        public RectTransform RectTransform => rectTransform;
        public Vector2 OriginalSizeDelta => originalSizeDelta;

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

            switch (playingCardInfo.DropZone.DropZoneType)
            {
                case DropZoneType.Return:
                    ReturnCardToPreviousPosition();
                    break;
                case DropZoneType.Unit:
                    gameManager.DeckManager.PlayCard(playingCardInfo, () => PlayingCardBehaviour(playingCardInfo));
                    break;
            }
        }

        public void ReturnCardToPreviousPosition() => animator.AnimateCardMove(gameObject, originalPosition, onComplete: OnEndReturningToHand);

        private void OnEndReturningToHand()
        {
            gameManager.DeckManager.SetCardNewParent(this, ParentToReturnTo, onComplete:() => positioner.PositionObjects());
            positionManager.ResetToStartPosition(this);
            positioner.PositionObjects();
        }

        /// <summary>
        /// Execute all necessary methods to play card
        /// </summary>
        /// <param name="playingCardInfo"></param>
        private void PlayingCardBehaviour(PlayingCardInfo playingCardInfo)
        {
            // Casting spell
            Spell spellToUse = spellFactory.GetSpellByName(cardData.Spell.SpellName);
            foreach (SpellEffect effect in spellToUse.SpellEffects)
            {
                effect.ApplyEffect(playingCardInfo.UnitTarget);
            }

            // Animate card position
            animator.AnimateCardMove(gameObject, playingCardInfo.TransformTarget.position,
                onComplete: () => gameManager.DeckManager.SetCardNewParent(playingCardInfo.Card, playingCardInfo.TransformTarget, false, () => positioner.PositionObjects()));

            // Position cards in hand
            positioner.PositionObjects();
        }

        /// <summary>
        /// Checking conditions to play card
        /// </summary>
        /// <returns></returns>
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
            gameManager = GameManager.Instance;
            positioner = gameManager.HandCardsPositioner;

            cardView = GetComponent<ICardView>();
            animator = GetComponent<CardAnimator>();
        }
    }
}
