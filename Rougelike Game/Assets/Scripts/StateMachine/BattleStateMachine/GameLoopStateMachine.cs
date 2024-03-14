using Cards;
using Managers;
using Units;

namespace StateMachine.BattleStateMachine
{
    public class GameLoopStateMachine : StateMachine<GameLoopStateMachine>
    {
        private GameManager gameManager;
        private DeckManager deckManager;
        private UnitsManager unitsManager;

        private EnemiesPack enemiesPack;

        public GameManager GameManager { get => gameManager; }
        public DeckManager DeckManager { get => deckManager; }
        public UnitsManager UnitsManager { get => unitsManager; }
        public EnemiesPack EnemiesPack { get => enemiesPack; }

        protected override void Awake()
        {
            if (gameManager == null)
            {
                gameManager = GameManager.Instance;
            }

            deckManager ??= gameManager.DeckManager;

            if (unitsManager == null)
            {
                unitsManager = gameManager.UnitsManager;
            }

            unitsManager.OnEnemiesCleared += PlayerWin;

            base.Awake();
        }

        public void PlayerWin()
        {
            SetState(typeof(WinState));
        }

        public void SetEnemiesPack(EnemiesPack enemiesPack)
        {
            this.enemiesPack = enemiesPack;
        }
    }
}
