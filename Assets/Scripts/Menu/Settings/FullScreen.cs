using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    private bool isFullscreen = true;

    private void Start()
    {
        Screen.fullScreen = isFullscreen;
    }
    public void openInFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
    }
}
