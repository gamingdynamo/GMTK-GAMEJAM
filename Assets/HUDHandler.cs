using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDHandler : MonoBehaviour
{
    public static HUDHandler Instance;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OverlayHUD;
    [SerializeField] private TextMeshProUGUI FlyCount;
    private bool Pause = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHUD()
    {
        FlyCount.text = GameManager.Instance.FlyCount.ToString();
    }

    public void ChangePauseState()
    {
        Pause = !Pause;
        Time.timeScale = Pause ? 0 : 1;
        PauseMenu.SetActive(Pause);
        OverlayHUD.SetActive(!Pause);
        Cursor.lockState = Pause ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = Pause;
    }
}
