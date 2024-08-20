using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region HUD
    [HideInInspector]public int FlyNeeded;
    [HideInInspector]public int FlyRequired;
    private int flyCount;
    public int FlyCount
    {
        get { return flyCount; }
        set
        {
          
            flyCount = value;
            FlyRequired = FlyNeeded - flyCount;
            if (HUDHandler.Instance == null) { return; }
            HUDHandler.Instance.UpdateHUD();
        }
    }

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

    private void Start()
    {
        FlyRequired = FlyNeeded;
    }


    public static float VectorToAngle(Vector2 dir)
    {
        return (dir.x < 0.0f ? -1.0f : 1.0f) * Vector2.Angle(Vector2.up, dir);
    }

    public static Vector2 AngleToVector(float angle)
    {
        return new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
    }
    
    public void LoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
    
}
