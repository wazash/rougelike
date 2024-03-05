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

        [SerializeField] private CardPositioningConfig handPositioningConfig;
        private CardPositioner handCardsPositioner;

        public DeckConfiguration DeckConfiguration => deckConfiguration;
        public Card CardPrefab => cardPrefab;
        public DeckManager DeckManager => deckManager;
        public DeckPositions DeckPositions => deckPositions;
        public CardPositioner HandCardsPositioner => handCardsPositioner;
        public CardPositioningConfig HandPositioningConfig => handPositioningConfig;

        private void Awake()
        {
            handCardsPositioner = new(deckPositions.HandDeckTransform.gameObject, handPositioningConfig);
            deckManager = new(deckPositions);
        }
    }
}
