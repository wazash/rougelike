using System;
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

            mainMenuManager.StartButton.onClick.AddListener(() => StartGame(parent));
            mainMenuManager.ExitButton.onClick.AddListener(ExitGame);
            mainMenuManager.CreditsButton.onClick.AddListener(OpenCredits);
            mainMenuManager.SettingsButton.onClick.AddListener(OpenSettings);
            mainMenuManager.BackButton.onClick.AddListener(BackToMainMenu);
        }

        override public void Exit()
        {
            base.Exit();

            HideAllMainMenuScreens();

            mainMenuManager.StartButton.onClick.RemoveAllListeners();
            mainMenuManager.ExitButton.onClick.RemoveAllListeners();
            mainMenuManager.CreditsButton.onClick.RemoveAllListeners();
            mainMenuManager.SettingsButton.onClick.RemoveAllListeners();
        }

        private void StartGame(GameLoopStateMachine machine)
        {
            machine.SetState(typeof(ChoosePlayerClassState));
        }

        private void ExitGame()
        {
            Debug.Log("Game is exiting");
            Application.Quit();
        }

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
