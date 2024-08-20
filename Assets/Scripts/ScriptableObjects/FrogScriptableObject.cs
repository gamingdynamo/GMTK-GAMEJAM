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

    [Header("Frog Tongue")]
    public float FrogTongueAimDistance = 50.0f;
    public float FrogTongueMaxLength = 5.0f;
    public float FrogTongueShootTime = 0.2f;
    public float FrogTongueHoldTime = 0.1f;
    public float FrogTongueRetrieveTime = 0.2f;
    public float FrogTongueRadiusScale = 0.1f;

    [Header("Aim Assist")]
    public float AimAssistAngle = 15.0f;
    public float AimAssistYOffset = 0.3f;
    public int AimAssistColliderNumber = 3;

    [Header("Frog Upgrade")]
    public float FrogScaleIncreaseAmount = 0.1f;
    public float FrogJumpTimeIncreaseAmount = 0.1f;
    public float FrogJumpYVelocityIncreaseAmount = 0.5f;
    public float FrogJumpXZVelocityIncreaseAmount = 0.5f;
    public float FrogTongueLengthIncreaseAmount = 0.5f;

    [Header("Frog Animation")]
    public float FrogHopAnimationSpeed = 2.0f;
    public float FrogJumpAnimationSpeed = 0.5f;

}
