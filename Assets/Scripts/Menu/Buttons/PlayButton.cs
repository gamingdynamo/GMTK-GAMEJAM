using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public int NumberOfSceneToLoad;
    public void startNewScene()
    {
        SceneManager.LoadScene(NumberOfSceneToLoad);
    }
}
