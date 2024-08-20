using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PullTonguableScriptableObject", menuName = "ScriptableObjects/PullTonguableScriptableObject")]
public class PullTonguableScriptableObject : ScriptableObject
{
    public int PullRequiredLevel = 0;
    public float PullForce = 50.0f;
    public float PullStopDistance = 1.0f;
    public bool IsMovable = false;
}
