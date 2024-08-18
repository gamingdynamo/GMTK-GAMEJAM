using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject[] SettingsObject, OtherObject;
    private bool isSettingsActive = false;


    public void openAndCloseSettings()
    {
        isSettingsActive = !isSettingsActive;
        for (int i = 0; i < SettingsObject.Length; i++)
        {
            SettingsObject[i].SetActive(isSettingsActive);
        }
        for (int i = 0; i < OtherObject.Length; i++)
        {
            OtherObject[i].SetActive(!isSettingsActive);
        }
    }
}
