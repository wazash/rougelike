using Sirenix.OdinInspector;
using Spells;
using Units;
using UnityEngine;

namespace Cards
{
    public class Card : Draggable
    {
        [InlineEditor]
        [SerializeField] private CardData cardData;

        [SerializeField] private ICardView cardView;

        private void Awake()
        {
            cardView = GetComponent<ICardView>();
        }

        public void PlayCard(Unit target)
        {
            if (cardData == null || cardData.Spell == null)
            {
                Debug.LogWarning("Card data or spell is missing!");
                return;
            }

            foreach (SpellEffect effect in cardData.Spell.SpellEffects)
            {
                effect.ApplyEffect(target);
            }

            Destroy(gameObject);
        }

        private void OnValidate()
        {
            cardView ??= GetComponent<ICardView>();

            if (cardData != null)
                cardView.UpdateVisuals(cardData);
        }
    }
}
