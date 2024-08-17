using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrogBehaviourScriptableObject", menuName = "ScriptableObjects/FrogBehaviourScriptableObject")]
public class FrogScriptableObject : ScriptableObject
{
    [Header("Frog Walk")]
    public float FrogWalkHeight = 0.5f;
    public float FrogWalkDistance = 0.2f;
    public float FrogWalkTimeInterval = 0.5f;
    public float FrogWalkCoolDown = 0.3f;

    [Header("Frog Jump")]
    public float FrogJumpLeaveGroundCoyoteTime = 0.05f;
    public float FrogJumpInputCoyoteTime = 0.05f;
    public float FrogMaxJumpTime = 1.0f;
    public float FrogJumpVerticleVelocity = 5.0f;
    public float FrogJumpForwardVelocity = 3.0f;
    public float FrogJumpFallControlVelocity = 0.1f;
}
