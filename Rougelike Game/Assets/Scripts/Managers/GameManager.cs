using Cards;
using Database;
using Map;
using NewSaveSystem;
using Sirenix.OdinInspector;
using StateMachine.BattleStateMachine;
using Units;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Title("Database")]
        [SerializeField] private ScriptableObjectDatabase database;
        [SerializeField] private Player playerPrefab;


        [Title("Managers")]
        [SerializeField] private UnitsManager unitsManager;
        [SerializeField] private UnitsGroundManager unitsGroundManager;
        [SerializeField] private MainMenuManager mainMenuManager;
        [SerializeField] private ClassSelectionManager classSelectionManager;
        [SerializeField] private MapManager mapManager;
        [ShowInInspector, HideReferenceObjectPicker] private DeckManager deckManager;

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
        public MainMenuManager MainMenuManager => mainMenuManager;
        public ClassSelectionManager ClassSelectionManager => classSelectionManager;
        public MapManager MapManager => mapManager;
        public DeckManager DeckManager => deckManager;
        public GameLoopStateMachine GameLoopStateMachine => gameLoopStateMachine;
        public StartingDeckConfig DeckConfiguration => deckConfiguration;
        public Card CardPrefab => cardPrefab;
        public DeckPositions DeckPositions => deckPositions;
        public ObjectPositioner HandCardsPositioner => handCardsPositioner;
        public ObjectPositioningConfig HandPositioningConfig => handPositioningConfig;

        public ScriptableObjectDatabase Database => database;
        public Player PlayerPrefab => playerPrefab;

        private void Awake()
        {
            if(database == null)
                database = Resources.Load<ScriptableObjectDatabase>("ScriptableObjectDatabase");

            handCardsPositioner = new(deckPositions.HandDeckTransform.gameObject, handPositioningConfig);
            deckManager = new(deckPositions);
        }
    }
}
