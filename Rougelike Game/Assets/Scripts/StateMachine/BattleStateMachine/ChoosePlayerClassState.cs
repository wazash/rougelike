using Managers;
using Map;
using Spells;
using System.Collections;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "ChoosePlayerClassState", menuName = "StateMachine/States/ChoosePlayerClassState")]
    public class ChoosePlayerClassState : State<GameLoopStateMachine>
    {
        private ClassSelectionManager classSelectionManager;
        //private UnitsManager unitsManager;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);

            //unitsManager = GameManager.Instance.UnitsManager;
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
            Debug.Log($"Registering {classSelectionManager.ClassSelectionWindows.Count} buttons");
            foreach (var classSelectionWindow in classSelectionManager.ClassSelectionWindows)
            {
                classSelectionWindow.ChooseButton.onClick.AddListener(() => OnClassChosen(classSelectionWindow));
                Debug.Log("Registered button", classSelectionWindow.ChooseButton);
            }
        }

        private void OnClassChosen(ClassSelectionWindow classSelectionWindow)
        {
            CoroutineRunner.Start(OnClassChosenRoutine(classSelectionWindow));
        }

        private IEnumerator OnClassChosenRoutine(ClassSelectionWindow classSelectionWindow)
        {
            var playerSpawnPoint = machine.GameManager.UnitsGroundManager.PlayerSpawnPoint;
            var playerParent = machine.GameManager.UnitsGroundManager.PlayerGround;

            yield return CoroutineRunner.Start(classSelectionWindow.PlayerData.SpawnPlayer(playerSpawnPoint, playerParent));
            machine.SetState(typeof(WorldMapState));
        }
    }

}
