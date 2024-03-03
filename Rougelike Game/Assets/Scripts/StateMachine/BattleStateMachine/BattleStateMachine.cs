using Cards;
using Managers;

namespace StateMachine.BattleStateMachine
{
    public class BattleStateMachine : StateMachine<BattleStateMachine>
    {
        private GameManager gameManager;
        private DeckManager deckManager;

        public GameManager GameManager { get => gameManager; }
        public DeckManager DeckManager { get => deckManager; }

        protected override void Awake()
        {
            if (gameManager == null)
            {
                gameManager = GameManager.Instance;
            }

            deckManager ??= gameManager.DeckManager;

            base.Awake();
        }
    }
}
