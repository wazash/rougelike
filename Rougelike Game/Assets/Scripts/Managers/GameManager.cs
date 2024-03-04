using Cards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private DeckConfiguration deckConfiguration;
        [SerializeField] private Card cardPrefab;

        [PropertySpace]

        [SerializeField] private DeckPositions deckPositions;

        [PropertySpace]

        [ShowInInspector]
        private DeckManager deckManager;

        [SerializeField] private HandCardsPositioningInfo handCardsPositioningInfo;
        private CardPositioner handCardsPositioner;

        public DeckConfiguration DeckConfiguration { get => deckConfiguration; }
        public Card CardPrefab { get => cardPrefab; }
        public DeckManager DeckManager { get => deckManager; }
        public DeckPositions DeckPositions { get => deckPositions; }
        public CardPositioner HandCardsPositioner { get => handCardsPositioner; }

        private void Awake()
        {
            handCardsPositioner = new(deckPositions.HandDeckTransform.gameObject/*, handCardsPositioningInfo*/);
            deckManager = new(deckPositions);
        }
    }
}
