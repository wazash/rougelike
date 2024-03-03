using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Cards
{
    public class DeckManager
    {
        [ShowInInspector, HideReferenceObjectPicker]
        private readonly Deck mainDeck;
        [ShowInInspector, HideReferenceObjectPicker]
        private readonly Deck gameplayDeck;
        [ShowInInspector, HideReferenceObjectPicker]
        private readonly Deck discardedDeck;
        [ShowInInspector, HideReferenceObjectPicker]
        private readonly Deck hand;

        public Deck MainDeck { get => mainDeck; }
        public Deck GameplayDeck { get => gameplayDeck; }
        public Deck DiscardedDeck { get => discardedDeck; }
        public Deck Hand { get => hand; }

        public DeckManager(DeckPositions deckPositions)
        {
            mainDeck = new Deck(deckPositions.MainDeckTransform);
            gameplayDeck = new Deck(deckPositions.GameplayDeckTransform);
            discardedDeck = new Deck(deckPositions.DiscardDeckTransform);
            hand = new Deck(deckPositions.HandDeckTransform);
        }

        public void InitializeMainDeck(DeckConfiguration startingDeckConfig, Card prefab, Transform deckTransform)
        {
            if (mainDeck == null)
            {
                Debug.LogWarning($"Main deck is missing!");
                return;
            }

            mainDeck.Clear();

            foreach (var cardData in startingDeckConfig.StartingCardsData)
            {
                Card card = SpawnCard(prefab, cardData, deckTransform);
                mainDeck.AddCard(card);
                card.gameObject.SetActive(false);
            }
        }

        public Card SpawnCard(Card cardPrefab, CardData data, Transform transform)
        {
            Card card = Object.Instantiate(cardPrefab, transform);
            card.Configure(data);
            return card;
        }

        public void PrepareGameplayDeck()
        {
            gameplayDeck.Clear();

            foreach (var card in mainDeck.GetCards())
            {
                gameplayDeck.AddCard(card);
            }

            gameplayDeck.Shuffle();
        }

        public IEnumerator DrawCardToHand(int numberOfCards)
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < numberOfCards; i++)
            {
                if (gameplayDeck.IsEmpty())
                {
                    RefillGameplayDeck();
                    if (gameplayDeck.IsEmpty())
                        break;
                }

                var card = gameplayDeck.DrawCard();
                hand.AddCard(card);
                card.gameObject.SetActive(true);

                card.GetComponent<CardAnimator>().AnimateCardMove(card.gameObject, hand.DeckPosition, onComplete: () => SetCardNewParent(card, hand.DeckPosition));

                yield return new WaitForSeconds(0.2f);
            }
        }

        public void SetCardNewParent(Card card, Transform newParent, bool showCard = true)
        {
            card.gameObject.transform.SetParent(newParent);
            card.gameObject.SetActive(showCard);
        }

        public void PlayCard(PlayingCardInfo info)
        {
            //info.TransformTarget = discardedDeckTransform;

            //info.Card.PlayCard(info);

            hand.RemoveCard(info.Card);
            discardedDeck.AddCard(info.Card);
        }

        public void DiscardCard(Card card)
        {
            hand.RemoveCard(card);
            discardedDeck.AddCard(card);
        }

        public void DiscardAllHandCards()
        {
            while (!hand.IsEmpty())
            {
                var card = hand.DrawCard();
                discardedDeck.AddCard(card);
            }
        }

        public void RefillGameplayDeck()
        {
            Debug.Log("DEBUG REFILLED");

            if (!discardedDeck.IsEmpty())
            {
                var card = discardedDeck.DrawCard();
                gameplayDeck.AddCard(card);
            }

            gameplayDeck.Shuffle();
        }
    }
}
