using Spells;
using Units;
using UnityEngine;

namespace Cards
{
    public class Card : Draggable
    {
        [SerializeField] private CardData cardData;

        [SerializeField] private CardView cardView;

        private void Awake()
        {
            cardView = GetComponent<CardView>();
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
                Destroy(gameObject);
            }
        }

        private void OnValidate()
        {
            if (cardView == null)
                cardView = GetComponent<CardView>();

            if (cardData != null)
                cardView.UpdateVisuals(cardData);
        }
    }
}
