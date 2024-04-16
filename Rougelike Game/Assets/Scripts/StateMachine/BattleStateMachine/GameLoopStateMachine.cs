using Cards;
using Managers;
using NewSaveSystem;
using System;
using Units;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    public struct StateMachineSaveData
    {
        public string CurrentStateName;
        public string PreviousStateName;

        public StateMachineSaveData(string currentState, string previousState)
        {
            CurrentStateName = currentState;
            PreviousStateName = previousState;
        }
    }

    public class GameLoopStateMachine : StateMachine<GameLoopStateMachine>, ISaveable
    {
        [SerializeField] private GameObject battleScreen;

        private GameManager gameManager;
        private DeckManager deckManager;
        private UnitsManager unitsManager;

        private EnemiesPack enemiesPack;

        public GameManager GameManager { get => gameManager; }
        public DeckManager DeckManager { get => deckManager; }
        public UnitsManager UnitsManager { get => unitsManager; }
        public EnemiesPack EnemiesPack { get => enemiesPack; }
        public GameObject BattleScreen { get => battleScreen; }

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

            SaveManager.RegisterSaveable(this);
        }

        public void PlayerWin()
        {
            SetState(typeof(WinState));
        }

        public void SetEnemiesPack(EnemiesPack enemiesPack)
        {
            this.enemiesPack = enemiesPack;
        }

        public string GetSaveID() => "GameLoopStateMachine";
        public Type GetDataType() => typeof(StateMachineSaveData);

        public object Save()
        {
            return new StateMachineSaveData(ActiveState.GetType().Name, PreviousState != null ? PreviousState.GetType().Name : "null");
        }

        public void Load(object saveData)
        { 
            StateMachineSaveData data = (StateMachineSaveData)saveData;

            State<GameLoopStateMachine> currentState = GetStateByName(data.CurrentStateName);
            SetState(currentState.GetType());
        }

        public State<GameLoopStateMachine> GetStateByType(Type type)
        {
            return MachineStates.Find(state => state.GetType() == type);
        }
    }
}
