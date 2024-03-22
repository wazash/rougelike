using Managers;
using NewSaveSystem;
using UnityEngine;

namespace StateMachine.BattleStateMachine
{
    [CreateAssetMenu(fileName = "MainMenuState", menuName = "StateMachine/States/MainMenuState")]
    public class MainMenuState : State<GameLoopStateMachine>
    {
        private MainMenuManager mainMenuManager;

        public override void Enter(GameLoopStateMachine parent)
        {
            base.Enter(parent);
            mainMenuManager = parent.GameManager.MainMenuManager;

            //mainMenuManager.MainMenuScreen.SetActive(true);
            DisplayContinueIfSaveExist();
            RegisterButtons(parent);
        }


        override public void Exit()
        {
            base.Exit();

            HideAllMainMenuScreens();
            UnregisterButtons();
        }

        private void DisplayContinueIfSaveExist() => mainMenuManager.CheckSaveGame(SaveManager.SavePath);
        private void RegisterButtons(GameLoopStateMachine parent)
        {
            mainMenuManager.NewGameButton.onClick.AddListener(() => NewGame(parent));
            mainMenuManager.ContinueButton.onClick.AddListener(LoadGame);
            mainMenuManager.ExitButton.onClick.AddListener(ExitGame);
            mainMenuManager.CreditsButton.onClick.AddListener(OpenCredits);
            mainMenuManager.SettingsButton.onClick.AddListener(OpenSettings);
            mainMenuManager.BackButton.onClick.AddListener(BackToMainMenu);
        }
        private void UnregisterButtons()
        {
            mainMenuManager.NewGameButton.onClick.RemoveAllListeners();
            mainMenuManager.ContinueButton.onClick.RemoveAllListeners();
            mainMenuManager.ExitButton.onClick.RemoveAllListeners();
            mainMenuManager.CreditsButton.onClick.RemoveAllListeners();
            mainMenuManager.SettingsButton.onClick.RemoveAllListeners();
            mainMenuManager.BackButton.onClick.RemoveAllListeners();
        }
        private void NewGame(GameLoopStateMachine machine) => machine.SetState(typeof(ChoosePlayerClassState));
        private void LoadGame()
        {
            // If Player is not spawned, create a new one
            if (GameManager.Instance.UnitsManager.Player == null)
            {
                Debug.Log("Player not found, creating a new one");
                machine.GameManager.UnitsManager.CreateEmptyPlayer();
            }

            SaveManager.LoadGame();
        }

        private void ExitGame() => Application.Quit();
        private void OpenCredits()
        {
            mainMenuManager.MainMenuScreen.SetActive(false);
            mainMenuManager.SettingsMenuScreen.SetActive(false);

            mainMenuManager.CreditsMenuScreen.SetActive(true);
            mainMenuManager.BackButton.gameObject.SetActive(true);
        }
        private void OpenSettings()
        {
            mainMenuManager.MainMenuScreen.SetActive(false);
            mainMenuManager.CreditsMenuScreen.SetActive(false);

            mainMenuManager.SettingsMenuScreen.SetActive(true);
            mainMenuManager.BackButton.gameObject.SetActive(true);
        }
        private void BackToMainMenu()
        {
            mainMenuManager.MainMenuScreen.SetActive(true);

            mainMenuManager.SettingsMenuScreen.SetActive(false);
            mainMenuManager.CreditsMenuScreen.SetActive(false);
            mainMenuManager.BackButton.gameObject.SetActive(false);
        }
        private void HideAllMainMenuScreens()
        {
            mainMenuManager.MainMenuScreen.SetActive(false);
            mainMenuManager.CreditsMenuScreen.SetActive(false);
            mainMenuManager.SettingsMenuScreen.SetActive(false);
            mainMenuManager.BackButton.gameObject.SetActive(false);
        }
    }
}
