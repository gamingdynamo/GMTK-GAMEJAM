using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject SettingsObject, OtherObject;
    private static bool isSettingsActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openAndCloseSettings();
            Time.timeScale = 0f;
        }
    }

    public void openAndCloseSettings()
    {
        isSettingsActive = !isSettingsActive;
        SettingsObject.SetActive(isSettingsActive);
        if (OtherObject != null)
        {
            OtherObject.SetActive(!isSettingsActive);
        }
    }
}
