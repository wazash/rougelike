using Cards;
using Sirenix.OdinInspector;
using StateMachine.BattleStateMachine;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Title("Managers")]
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] 
        private UnitsGroundManager unitsGroundManager;
        [SerializeField] 
        private MainMenuManager mainMenuManager;
        [SerializeField]
        private ClassSelectionManager classSelectionManager;
        [ShowInInspector, HideReferenceObjectPicker] 
        private DeckManager deckManager;

        [Title("State Machine")]
        [SerializeField] private GameLoopStateMachine gameLoopStateMachine;

        [Title("Configs")]
        [SerializeField] private StartingDeckConfig deckConfiguration;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private DeckPositions deckPositions;
        [SerializeField] private ObjectPositioningConfig handPositioningConfig;
        private ObjectPositioner handCardsPositioner;

        public UnitsManager UnitsManager => unitsManager;
        public UnitsGroundManager UnitsGroundManager => unitsGroundManager;
        public DeckManager DeckManager => deckManager;
        public MainMenuManager MainMenuManager => mainMenuManager;
        public ClassSelectionManager ClassSelectionManager => classSelectionManager;
        public GameLoopStateMachine GameLoopStateMachine => gameLoopStateMachine;
        public StartingDeckConfig DeckConfiguration => deckConfiguration;
        public Card CardPrefab => cardPrefab;
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
