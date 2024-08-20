using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        GameManager.Instance.LoadScene(sceneIndex);
    }
}
