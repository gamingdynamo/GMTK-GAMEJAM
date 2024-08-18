using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
    private Button backButton;
    private VisualElement mainMenuPanel;
    private VisualElement optionPanel;

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
        backButton = root.Q<Button>("back-button");
        mainMenuPanel = root.Q<VisualElement>("button-container");
        optionPanel = root.Q<VisualElement>("option-panel");


        startButton.clicked += StartGame;
        settingsButton.clicked += OpenSettings;
        exitButton.clicked += ExitGame;
        backButton.clicked += OpenMainMenu;

        // Ensure the main menu is visible and options are hidden initially
        OpenMainMenu();
    }

    private void OnDisable()
    {
        startButton.clicked -= StartGame;
        settingsButton.clicked -= OpenSettings;
        exitButton.clicked -= ExitGame;
        backButton.clicked -= OpenMainMenu;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("TestingScene");
    }

    private void OpenSettings()
    {
        Debug.Log("Opening settings...");
        mainMenuPanel.style.display = DisplayStyle.None;
        optionPanel.style.display = DisplayStyle.Flex;
    }

    private void OpenMainMenu()
    {
        mainMenuPanel.style.display = DisplayStyle.Flex;
        optionPanel.style.display = DisplayStyle.None;
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

    // Additional methods for settings functionality
    private void ApplySettings()
    {
        // Implement logic to apply settings
        Debug.Log("Applying settings...");
    }

    private void ResetSettings()
    {
        // Implement logic to reset settings to default
        Debug.Log("Resetting settings to default...");
    }
}
