using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Deck
    {
        public int Count => cards.Count;

        [ShowInInspector]
        private readonly List<Card> cards = new();

        public Transform DeckPosition { get; set; }

        public Deck(Transform deckPosition)
        {
            DeckPosition = deckPosition;
        }

        public void AddCard(Card card) => cards.Add(card);

        public void AddCards(List<Card> cards) => cards.AddRange(cards);

        public void RemoveCard(Card card) => cards.Remove(card);

        public Card DrawCard()
        {
            if (IsEmpty()) return null;

            Card card = cards[0];
            RemoveCard(card);
            return card;
        }

        public List<Card> GetCards() => cards;

        public void Clear() => cards.Clear();

        public bool IsEmpty() => cards.Count == 0;

        public void Shuffle()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card temp = cards[i];
                int randomIndex = Random.Range(i, cards.Count);
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
    }
}
