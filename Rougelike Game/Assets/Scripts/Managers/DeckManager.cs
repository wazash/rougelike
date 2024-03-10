using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

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

        private ObjectPositioner positioner;

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

            positioner = GameManager.Instance.HandCardsPositioner;
        }

        /// <summary>
        /// Spawning starting deck cards and assing them into Main Deck
        /// </summary>
        /// <param name="startingDeckConfig"></param>
        /// <param name="prefab"></param>
        /// <param name="deckTransform"></param>
        public void InitializeMainDeck(StartingDeckConfig startingDeckConfig, Card prefab, Transform deckTransform)
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

        /// <summary>
        /// Spawn new card from card prefab base on card data
        /// </summary>
        /// <param name="cardPrefab"></param>
        /// <param name="data"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Card SpawnCard(Card cardPrefab, CardData data, Transform transform)
        {
            Card card = UnityEngine.Object.Instantiate(cardPrefab, transform);
            card.Configure(data);
            return card;
        }

        /// <summary>
        /// Copy cards from Main Deck into Gameplay Deck and then shuffle them
        /// </summary>
        public void PrepareGameplayDeck()
        {
            gameplayDeck.Clear();

            foreach (var card in mainDeck.GetCards())
            {
                gameplayDeck.AddCard(card);
            }

            gameplayDeck.Shuffle();
        }

        /// <summary>
        /// Drawing card from Gameplay Deck and assing them into Hand Deck
        /// </summary>
        /// <param name="numberOfCards"></param>
        /// <returns></returns>
        public IEnumerator DrawCardToHand(int numberOfCards, float timeBetweenDraws)
        {
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < numberOfCards; i++)
            {
                if (gameplayDeck.IsEmpty())
                {
                    yield return CoroutineRunner.Start(RefillGameplayDeck());
                    if (gameplayDeck.IsEmpty())
                        break;
                }

                var card = gameplayDeck.DrawCard();
                hand.AddCard(card);
                card.gameObject.SetActive(true);

                card.GetComponent<CardAnimator>().AnimateCardMove(card.gameObject, hand.DeckTransform.position, 
                    onComplete: () => SetCardNewParent(card, hand.DeckTransform, onComplete: () => positioner.PositionObjects()));

                yield return new WaitForSeconds(timeBetweenDraws);
            }
        }

        /// <summary>
        /// Setting card the new parrent and hide or show (default) that card
        /// </summary>
        /// <param name="card"></param>
        /// <param name="newParent"></param>
        /// <param name="showCard"></param>
        public void SetCardNewParent(Card card, Transform newParent, bool showCard = true, Action onComplete = null)
        {
            card.gameObject.transform.SetParent(newParent);
            card.gameObject.SetActive(showCard);
            onComplete?.Invoke();
        }

        /// <summary>
        /// Moving card from Hand Deck into Discarded Deck
        /// </summary>
        /// <param name="info"></param>
        /// <param name="onComplete"></param>
        public void PlayCard(PlayingCardInfo info, Action onComplete)
        {
            hand.RemoveCard(info.Card);
            discardedDeck.AddCard(info.Card);

            onComplete?.Invoke();
        }

        /// <summary>
        /// Discarded given card
        /// </summary>
        /// <param name="card"></param>
        public IEnumerator DiscardCard(Card card)
        {
            yield return new WaitForEndOfFrame();

            bool animationCompleted = false;
            float timeout = 10f;

            if (!hand.IsEmpty())
            {
                card.GetComponent<CardAnimator>().AnimateCardMove(card.gameObject, discardedDeck.DeckTransform.position, onComplete: () => animationCompleted = true);
            }

            float startTime = Time.time;
            yield return new WaitUntil(() =>  animationCompleted || Time.time - startTime > timeout);

            hand.RemoveCard(card);
            discardedDeck.AddCard(card);
            SetCardNewParent(card, discardedDeck.DeckTransform, false, () => positioner.PositionObjects());
        }

        /// <summary>
        /// Discarded all cards in Hand Deck
        /// </summary>
        public IEnumerator DiscardAllHandCards()
        {
            yield return new WaitForEndOfFrame();

            List<Card> cardsToDiscard = new(hand.GetCards());

            foreach (Card card in cardsToDiscard) 
            {
                yield return DiscardCard(card);
            }
        }

        public IEnumerator MoveCardFromDiscardedToGameplay(Card card)
        {
            card.gameObject.SetActive(true);
            card.GetComponent<CardAnimator>().AnimateCardMove(card.gameObject, gameplayDeck.DeckTransform.position, 
                onComplete: () => 
                {
                    discardedDeck.RemoveCard(card);
                    gameplayDeck.AddCard(card);
                    SetCardNewParent(card, gameplayDeck.DeckTransform, false);
                });
            yield return null;
        }

        /// <summary>
        /// Shuffle cards from Discarded Deck into Gameplay Deck
        /// </summary>
        /// <param name="durationBetween">Time in seconds between cards animation move to gameplay deck</param>
        /// <param name="durationAfter">Time in seconds after all cards moved to gameplay deck</param>
        /// <returns></returns>
        public IEnumerator RefillGameplayDeck(float durationBetween = 0.1f, float durationAfter = 0.3f)
        {
            if (!discardedDeck.IsEmpty())
            {
                List<Card> cardsToRefill = new(discardedDeck.GetCards());

                foreach (var card in cardsToRefill)
                {
                    CoroutineRunner.Start(MoveCardFromDiscardedToGameplay(card));
                    yield return new WaitForSeconds(durationBetween);
                }
            }

            gameplayDeck.Shuffle();
            yield return new WaitForSeconds(durationAfter);
        }
    }
}
