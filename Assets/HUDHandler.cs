using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class HUDHandler : MonoBehaviour
{
    public static HUDHandler Instance { get; private set; }
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OverlayHUD;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private TextMeshProUGUI FlyCount;
    [SerializeField]
    private PopUpHUD popUpHUD;
    private bool Pause = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateHUD()
    {
        FlyCount.text = GameManager.Instance.FlyCount.ToString();
        UpdatePopUpMessage();
    }

    public void ChangePauseState()
    {
        Pause = !Pause;
        Time.timeScale = Pause ? 0 : 1;
        PauseMenu.SetActive(Pause);
        OverlayHUD.SetActive(!Pause);
        Cursor.lockState = Pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = Pause;
        InputHandler.Instance.EnablePlayerControl(!Pause);
    }

    public void FinishLevel()
    {
        OverlayHUD.SetActive(false);
        WinScreen.SetActive(true);
        Invoke(nameof(BackMainMenu), 3.0f);
    }

    public void StartLevel()
    {
        OverlayHUD.SetActive(true);
    }

    public void BackMainMenu()
    {
        WinScreen.SetActive(false);
        PauseMenu.SetActive(false);
        Cursor.visible = true;
        Pause = false;
        InputHandler.Instance.EnablePlayerControl(true);
        InputHandler.Instance.ControlsActive(false);
        GameManager.Instance.LoadScene(0);
    }

    public void ShowPopUpHud(Vector3 popUpLocation)
    {
        popUpHUD.ShowPopUpHud(popUpLocation);
        UpdatePopUpMessage();
    }
    
    public void UpdatePopUpMessage()
    {
        popUpHUD.UpdatePopupMessage(GameManager.Instance.FlyRequired + " More " + (GameManager.Instance.FlyRequired > 1 ? "Flies" : "Fly") + " Needed");
    }

    public void HidePopUpHud()
    {
        popUpHUD.HidePopUpHudDummy();
    }
}
