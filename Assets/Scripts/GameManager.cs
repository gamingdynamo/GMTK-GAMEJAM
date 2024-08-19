using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
}
