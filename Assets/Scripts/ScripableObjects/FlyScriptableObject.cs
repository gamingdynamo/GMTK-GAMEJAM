using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyBehaviourScriptableObject", menuName = "ScriptableObjects/FlyBehaviourScriptableObject")]
public class FlyScriptableObject : ScriptableObject
{
    [Header("Patroll")]
    public float FlyPatrollRadius = 0.5f;
    public float FlyPatrollTimeInterval = 5.0f;
    public bool FlyPatrollClockWise = true;

    [Header("FlyRandom")]
    public int RandomPosMinNumber = 2;
    public int RandomPosMaxNumber = 5;
    public float RandomPosReachTime = 0.5f;
    public float RandomPosMinDistance = 0.1f;
    public float RandomPosMaxDistance = 0.3f;
    public float RandomPosMaxAngle = 60.0f;
    public float RandomPosPauseTime = 0.2f;
    public float FlyRestTime = 5.0f;
}
