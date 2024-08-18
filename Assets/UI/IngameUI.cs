using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button mainMenuButton;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        mainMenuButton = root.Q<Button>("main-menu-button");

        mainMenuButton.clicked += OpenMainMenu;
    }

    private void OnDisable()
    {
        mainMenuButton.clicked -= OpenMainMenu;
    }

    private void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
