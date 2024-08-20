using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject PauseMain;
    private bool togglePauseMenu = false;

    #region HUD
    private int flyCount;
    public int FlyCount
    {
        get { return flyCount; }
        set
        {
            flyCount = value;
            UpdateHUD();
        }
    }

    [SerializeField] private TextMeshProUGUI FlyCountText;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }




    public static float VectorToAngle(Vector2 dir)
    {
        return (dir.x < 0.0f ? -1.0f : 1.0f) * Vector2.Angle(Vector2.up, dir);
    }

    public static Vector2 AngleToVector(float angle)
    {
        return new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
    }


    private void UpdateHUD()
    {
        FlyCountText.text = flyCount.ToString();
    }

    public void ShowPauseMenu()
    {   
        togglePauseMenu = !togglePauseMenu;
        Cursor.lockState = togglePauseMenu ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = togglePauseMenu;
        Time.timeScale = togglePauseMenu ? 0.0f : 1.0f;
        if (PauseMain == null) { return; }
        PauseMain.SetActive(togglePauseMenu);
    }
}
