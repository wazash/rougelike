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

        public DeckConfiguration DeckConfiguration { get => deckConfiguration; }
        public Card CardPrefab { get => cardPrefab; }
        public DeckManager DeckManager { get => deckManager; }
        public DeckPositions DeckPositions { get => deckPositions; }

        private void Awake() => deckManager = new(deckPositions);
    }
}
