using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonguable : MonoBehaviour
{
    private FrogTongueAssist tongueAssist = null;

    public void GotTongued()
    {
        Debug.Log(gameObject.name + " Got tongued");
    }

    public void SetTougueAssist(FrogTongueAssist tongueAssist)
    {
        this.tongueAssist = tongueAssist;
    }
}
