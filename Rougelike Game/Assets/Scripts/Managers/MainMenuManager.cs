using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject settingsMenuScreen;
    [SerializeField] private GameObject creditsMenuScreen;

    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backButton;

    public GameObject MainMenuScreen => mainMenuScreen;
    public GameObject SettingsMenuScreen => settingsMenuScreen;
    public GameObject CreditsMenuScreen => creditsMenuScreen;

    public Button StartButton => startButton;
    public Button SettingsButton => settingsButton;
    public Button CreditsButton => creditsButton;
    public Button ExitButton => exitButton;
    public Button BackButton => backButton;
}
