using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
    private VisualElement MainMenuPanel;
    private VisualElement OptionPanel;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        startButton = root.Q<Button>("start-button");
        settingsButton = root.Q<Button>("settings-button");
        exitButton = root.Q<Button>("exit-button");

        startButton.clicked += StartGame;
        settingsButton.clicked += OpenSettings;
        exitButton.clicked += ExitGame;
    }

    private void OnDisable()
    {
        startButton.clicked -= StartGame;
        settingsButton.clicked -= OpenSettings;
        exitButton.clicked -= ExitGame;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("TestingScene");
    }

    private void OpenSettings()
    {
        Debug.Log("Opening settings...");

    }
    private void OpenMainMenu()
    {
        MainMenuPanel;
    }

    private void ExitGame()
    {
        Debug.Log("Exiting the game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
