using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField]
    public static float setedVolume;

    private void Start()
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().volume = setedVolume;
        }
    }
    public void setVolume()
    {
        Slider slider = GetComponent<Slider>();
        setedVolume = slider.value;
    }
}
