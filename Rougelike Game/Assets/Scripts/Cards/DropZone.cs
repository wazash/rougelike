using Managers;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private DropZoneType dropZoneType;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }

        public DropZoneType DropZoneType { get => dropZoneType; }

        public void OnDrop(PointerEventData eventData)
        {
            // check if dragging object is a Card
            if (eventData.pointerDrag.TryGetComponent(out Card draggingCard))
            {
                PlayingCardInfo cardInfo = new()
                {
                    Card = draggingCard,
                    DropZone = this,
                    TransformTarget = gameManager.DeckManager.DiscardedDeck.DeckTransform
                };

                if (TryGetComponent<Unit>(out var unit))
                {
                    cardInfo.UnitTarget = unit;
                }

                cardInfo.Card.PlayCard(cardInfo);
                return;
            }
        }
    } 
}
