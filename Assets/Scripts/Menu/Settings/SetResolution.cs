using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class AllResolutions
{
    [SerializeField]
    public int ResolutionX;
    [SerializeField]
    public int ResolutionY;
}

public class SetResolution : MonoBehaviour
{
    [SerializeField]
    public AllResolutions[] Resolutions;
    private int numberOfResolution;
    [SerializeField]
    private TextMeshProUGUI ResolutionText;

    private void Start()
    {
        ResolutionText.text = (Resolutions[numberOfResolution].ResolutionX.ToString() + "x" + Resolutions[numberOfResolution].ResolutionY.ToString());
    }
    public void setResolution()
    {
        Screen.SetResolution(Resolutions[numberOfResolution].ResolutionX, Resolutions[numberOfResolution].ResolutionY, true);
    }

    public void numberOfResolutionPlus()
    {
        if (numberOfResolution != Resolutions.Length-1)
        {
            numberOfResolution++;
        }
        else
        {
            numberOfResolution = 0;
        }
        ResolutionText.text = (Resolutions[numberOfResolution].ResolutionX.ToString() + "x" + Resolutions[numberOfResolution ].ResolutionY.ToString());
        setResolution();
    }

    public void numberOfResolutionMinus()
    {
        if (numberOfResolution != 0)
        {
            numberOfResolution--;
        }
        else
        {
            numberOfResolution = Resolutions.Length-1;
        }
        ResolutionText.text = (Resolutions[numberOfResolution].ResolutionX.ToString() + "x" + Resolutions[numberOfResolution].ResolutionY.ToString());
        setResolution();
    }
}
