using Managers;
using System.Collections;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "ChoosePlayerClassState", menuName = "StateMachine/States/ChoosePlayerClassState")]
    public class ChoosePlayerClassState : State<GameLoopStateMachine>
    {
        private ClassSelectionManager classSelectionManager;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            classSelectionManager = GameManager.Instance.ClassSelectionManager;

            classSelectionManager.ClassSelectionScreen.SetActive(true);
            classSelectionManager.RegisterClassSelectionWindows();
            RegisterButtons();
        }

        public override void Exit()
        {
            base.Exit();

            classSelectionManager.ClassSelectionScreen.SetActive(false);
        }

        private void RegisterButtons()
        {
            foreach (var classSelectionWindow in classSelectionManager.ClassSelectionWindows)
            {
                classSelectionWindow.ChooseButton.onClick.AddListener(() => OnClassChosen(classSelectionWindow));
            }
        }

        private void OnClassChosen(ClassSelectionWindow classSelectionWindow) 
            => CoroutineRunner.Start(OnClassChosenRoutine(classSelectionWindow));

        private IEnumerator OnClassChosenRoutine(ClassSelectionWindow classSelectionWindow)
        {
            var playerSpawnPoint = machine.GameManager.UnitsGroundManager.PlayerSpawnPoint;
            var playerParent = machine.GameManager.UnitsGroundManager.PlayerGround; 

            // Add starting deck cards to main deck
            machine.DeckManager.InitializeMainDeck(classSelectionWindow.PlayerData.StartingDeckConfig,
                                                   machine.GameManager.CardPrefab,
                                                   machine.GameManager.DeckPositions.MainDeckTransform);

            // Spawn player
            yield return CoroutineRunner.Start(classSelectionWindow.PlayerData.SpawnPlayer(playerSpawnPoint, playerParent));
            machine.SetState(typeof(WorldMapState));
        }
    }
}
