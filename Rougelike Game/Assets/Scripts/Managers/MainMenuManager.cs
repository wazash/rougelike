using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject settingsMenuScreen;
    [SerializeField] private GameObject creditsMenuScreen;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backButton;

    public GameObject MainMenuScreen => mainMenuScreen;
    public GameObject SettingsMenuScreen => settingsMenuScreen;
    public GameObject CreditsMenuScreen => creditsMenuScreen;

    public Button NewGameButton => newGameButton;
    public Button ContinueButton => continueGameButton;
    public Button SettingsButton => settingsButton;
    public Button CreditsButton => creditsButton;
    public Button ExitButton => exitButton;
    public Button BackButton => backButton;

    public void CheckSaveGame(string savePath)
    {
        if(File.Exists(savePath))
            continueGameButton.gameObject.SetActive(true);
        else
            continueGameButton.gameObject.SetActive(false);
    }
}
