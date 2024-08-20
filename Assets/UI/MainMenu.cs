using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class MainMenu : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;
    private Button settingsButton;
    private Button exitButton;
    private Button backButton;
    private Button saveButton;
    private VisualElement mainMenuPanel;
    private VisualElement optionPanel;

    private DropdownField resolutionSelectDropdown;
    private TextField resolutionWidthField;
    private TextField resolutionHeightField;
    private DropdownField windowModeDropdown;
    private DropdownField screenSelectDropdown;
    private DropdownField aaDropdown;
    private SliderInt aaMultiplierSlider;
    private DropdownField graphicsQualityDropdown;
    private Label revertWarning;
    private DropdownField vsyncDropdown;
    private Slider soundVolumeSlider;
    private Slider musicVolumeSlider;

    private Coroutine revertSettingsCoroutine;


    public UniversalRenderPipelineAsset urpAsset;
    public AudioMixer audioMixer;

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
        saveButton = root.Q<Button>("save-button");
        mainMenuPanel = root.Q<VisualElement>("button-container");
        optionPanel = root.Q<VisualElement>("option-panel");

        // Initialize settings UI elements
        resolutionWidthField = root.Q<TextField>("resolution-width");
        resolutionHeightField = root.Q<TextField>("resolution-height");
        windowModeDropdown = root.Q<DropdownField>("window-mode-select");
        resolutionSelectDropdown = uiDocument.rootVisualElement.Q<DropdownField>("resolution-select");
        screenSelectDropdown = uiDocument.rootVisualElement.Q<DropdownField>("screen-select");
        aaDropdown = root.Q<DropdownField>("aa-select");
        aaMultiplierSlider = root.Q<SliderInt>("aa-multiplier");
        graphicsQualityDropdown = root.Q<DropdownField>("graphics-select");
        vsyncDropdown = uiDocument.rootVisualElement.Q<DropdownField>("vsync-select");
        soundVolumeSlider = root.Q<Slider>("sound-volume-slider");
        musicVolumeSlider = root.Q<Slider>("music-volume-slider");

        revertWarning = root.Q<Label>("revert-warning");

        startButton.clicked += StartGame;
        settingsButton.clicked += OpenSettings;
        exitButton.clicked += ExitGame;
        backButton.clicked += OpenMainMenu;
        saveButton.clicked += ApplySettings;


        // Add change listeners
        soundVolumeSlider.RegisterValueChangedCallback(OnSoundVolumeChanged);
        musicVolumeSlider.RegisterValueChangedCallback(OnMusicVolumeChanged);
        // Add event handler for resolution dropdown
        resolutionSelectDropdown.RegisterValueChangedCallback(evt => UpdateResolutionFields(evt.newValue));

        // Ensure the main menu is visible and options are hidden initially
        OpenMainMenu();
        InitializeSettings();
    }

    private void OnDisable()
    {
        startButton.clicked -= StartGame;
        settingsButton.clicked -= OpenSettings;
        exitButton.clicked -= ExitGame;
        backButton.clicked -= OpenMainMenu;
        saveButton.clicked -= ApplySettings;
        soundVolumeSlider.UnregisterValueChangedCallback(OnSoundVolumeChanged);
        musicVolumeSlider.UnregisterValueChangedCallback(OnMusicVolumeChanged);

    }

    private void StartGame()
    {
        SceneManager.LoadScene("TestingScene");
    }

    private void OpenSettings()
    {
        mainMenuPanel.style.display = DisplayStyle.None;
        optionPanel.style.display = DisplayStyle.Flex;
        LoadCurrentSettings();
    }

    private void OpenMainMenu()
    {
        if (revertSettingsCoroutine != null) {
            StopCoroutine(revertSettingsCoroutine);
            revertWarning.style.display = DisplayStyle.None;
        }
        mainMenuPanel.style.display = DisplayStyle.Flex;
        optionPanel.style.display = DisplayStyle.None;
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void InitializeSettings()
    {
        // Set up AA dropdown options
        aaDropdown.choices = new List<string> { "Off", "2x", "4x", "8x"};
        windowModeDropdown.choices = new List<string> { "ExclusiveFullScreen", "Fullscreen", "Windowed", "Borderless" };

        // Set up graphics quality dropdown options
        graphicsQualityDropdown.choices = QualitySettings.names.ToList();

        // Initialize VSync dropdown
        vsyncDropdown.choices = new List<string> { "Off", "Every VBlank", "Every Second VBlank" };
        vsyncDropdown.index = 0;

        // Initialize screen selection dropdown
        List<string> screenNames = new List<string>();
        for (int i = 0; i < Display.displays.Length; i++)
        {
            screenNames.Add($"Screen {i + 1}");
        }
        screenSelectDropdown.choices = screenNames;
        screenSelectDropdown.index = 0;
        UpdateResolutionDropdown();
    }


    private void UpdateResolutionDropdown()
    {
        List<string> resolutions = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            resolutions.Add($"{resolution.width}x{resolution.height}");
        }
        resolutionSelectDropdown.choices = resolutions;
    }

    private void UpdateResolutionFields(string resolution)
    {
        var parts = resolution.Split('x');
        if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
        {
            resolutionWidthField.value = width.ToString();
            resolutionHeightField.value = height.ToString();
        }
    }


    private void LoadCurrentSettings()
    {
        // Load current resolution
        resolutionWidthField.value = Screen.width.ToString();
        resolutionHeightField.value = Screen.height.ToString();

        // Load current AA setting
        int currentAA = urpAsset.msaaSampleCount;
        aaDropdown.index = currentAA switch
        {
            1 => 0, // Off
            2 => 1,
            4 => 2,
            8 => 3,
            _ => 0
        };

        // Load current graphics quality
        graphicsQualityDropdown.index = QualitySettings.GetQualityLevel();

        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 100f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 100f);

        soundVolumeSlider.value = soundVolume;
        musicVolumeSlider.value = musicVolume;

    }

    private void ApplySettings()
    {
        bool resolutionChanged = false;
        int width = Screen.width;
        int height = Screen.height;

        FullScreenMode fullScreenMode = windowModeDropdown.index switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            3 => FullScreenMode.MaximizedWindow,
            _ => FullScreenMode.Windowed
        };

        if (fullScreenMode == FullScreenMode.Windowed || fullScreenMode == FullScreenMode.MaximizedWindow)
        {
            // For windowed modes, use the resolution from input fields
            if (int.TryParse(resolutionWidthField.value, out int inputWidth) &&
                int.TryParse(resolutionHeightField.value, out int inputHeight))
            {
                width = inputWidth;
                height = inputHeight;
            }
        }
        else
        {
            // For fullscreen modes, use the current display's resolution
            width = Screen.currentResolution.width;
            height = Screen.currentResolution.height;
        }

        resolutionChanged = (width != Screen.width || height != Screen.height || fullScreenMode != Screen.fullScreenMode);

        // Apply the new resolution and fullscreen mode
        Screen.SetResolution(width, height, fullScreenMode);

        // Apply AA
        int aaLevel = aaDropdown.index switch
        {
            0 => 1, // Off
            1 => 2,
            2 => 4,
            3 => 8,
            _ => 1
        };
        urpAsset.msaaSampleCount = aaLevel;

        // Apply VSync
        QualitySettings.vSyncCount = vsyncDropdown.index;

        // Apply graphics quality
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.index);

        // You might need to call this to ensure changes take effect
        QualitySettings.renderPipeline = urpAsset;

        if (resolutionChanged)
        {
            StartRevertCountdown();
        }

        // Update the resolution fields to reflect the actual applied resolution
        resolutionWidthField.value = width.ToString();
        resolutionHeightField.value = height.ToString();
    }
    private void StartRevertCountdown()
    {
        if (revertSettingsCoroutine != null)
        {
            StopCoroutine(revertSettingsCoroutine);
        }
        revertSettingsCoroutine = StartCoroutine(RevertSettingsCountdown());
    }

    private IEnumerator RevertSettingsCountdown()
    {
        revertWarning.style.display = DisplayStyle.Flex;
        float timeLeft = 15f;

        while (timeLeft > 0)
        {
            revertWarning.text = $"Settings will revert in {timeLeft:F0} seconds if not go back";
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        RevertSettings();
    }

    private void RevertSettings()
    {
        LoadCurrentSettings();
        ApplySettings();
        revertWarning.style.display = DisplayStyle.None;
    }
    private void ResetSettings()
    {
        // Reset to default settings
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(640, 480, Screen.fullScreen);
        QualitySettings.antiAliasing = 0;
        QualitySettings.SetQualityLevel(0); // Lowest quality

        LoadCurrentSettings(); // Reload UI to reflect changes
        Debug.Log("Settings reset to default");
    }

    // You might want to add this method to handle resolution changes at runtime
    private void OnResolutionChanged(Resolution resolution)
    {
        resolutionWidthField.value = resolution.width.ToString();
        resolutionHeightField.value = resolution.height.ToString();
    }

     void OnSoundVolumeChanged(ChangeEvent<float> evt)
    {
        // Update your sound volume here, for example:
        ApplyVolumeSettings(evt.newValue, musicVolumeSlider.value);
        // Save the setting
        PlayerPrefs.SetFloat("SoundVolume", evt.newValue);
    }

    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        // Update your music volume here
        ApplyVolumeSettings(soundVolumeSlider.value, evt.newValue);
        // Save the setting
        PlayerPrefs.SetFloat("MusicVolume", evt.newValue);
    }

    void ApplyVolumeSettings(float soundVolume, float musicVolume)
    {
        // This is where you would actually change the volume in your game
        // Assuming you have an AudioMixer named "MainMixer" with exposed parameters
        // AudioMixer audioMixer = Resources.Load<AudioMixer>("MainMixer");
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SoundVolume", Mathf.Log10(soundVolume / 100) * 20);
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume / 100) * 20);
        }

        // For now, just log for demonstration
        Debug.Log($"Sound Volume Set to: {soundVolume}, Music Volume Set to: {musicVolume}");
        // Save sound and music volume levels
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        PlayerPrefs.Save();
    }
}
