using Cards;
using Sirenix.OdinInspector;
using StateMachine.BattleStateMachine;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private StartingDeckConfig deckConfiguration;
        [SerializeField] private Card cardPrefab;

        [PropertySpace]

        [SerializeField] private DeckPositions deckPositions;

        [PropertySpace]

        [ShowInInspector]
        private DeckManager deckManager;

        [SerializeField] private ObjectPositioningConfig handPositioningConfig;
        private ObjectPositioner handCardsPositioner;

        public StartingDeckConfig DeckConfiguration => deckConfiguration;
        public Card CardPrefab => cardPrefab;
        public DeckManager DeckManager => deckManager;
        public DeckPositions DeckPositions => deckPositions;
        public ObjectPositioner HandCardsPositioner => handCardsPositioner;
        public ObjectPositioningConfig HandPositioningConfig => handPositioningConfig;

        private void Awake()
        {
            handCardsPositioner = new(deckPositions.HandDeckTransform.gameObject, handPositioningConfig);
            deckManager = new(deckPositions);
        }
    }
}
