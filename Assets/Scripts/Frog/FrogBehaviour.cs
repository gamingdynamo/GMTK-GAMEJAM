using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    public static FrogBehaviour Instance { get; private set; }

    [SerializeField]
    private FrogScriptableObject frogScripObj;
    [SerializeField]
    private Rigidbody frogRig;
    [SerializeField]
    private Animator frogAnmt;

    private float WalkFormulaA = 0.0f;
    private float WalkFormulaC = 0.0f;

    private Vector2 inputDirection = Vector2.zero;
    private Vector3 frogWalkDirection = Vector3.zero;
    private FrogWalkState frogCurrWalkState = FrogWalkState.Resting;
    private float frogWalkPastTime = 0.0f;
    private float frogWalkTimer = 0.0f;
    private Vector3 cameraForward = Vector3.zero;

    private bool frogOnGround = false;
    private float leaveGroundTimer = 0.0f;
    private float jumpInputTimer = 0.0f;
    private float jumpTimer = 0.0f;
    private Vector2 jumpDirection = Vector2.zero;

    private void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("ERROR: Frog Singleton Already Exsist");
            Destroy(gameObject);
            return;
        }

        WalkFormulaA = frogScripObj.FrogWalkHeight / (frogScripObj.FrogWalkDistance * frogScripObj.FrogWalkDistance * 0.25f);
        WalkFormulaC = frogScripObj.FrogWalkHeight;
    }

    private void FixedUpdate()
    {
        FrogFixedUpdate();
    }

    private void FrogFixedUpdate()
    {
        leaveGroundTimer = Mathf.Clamp(leaveGroundTimer - Time.fixedDeltaTime, 0.0f, Mathf.Infinity);
        jumpInputTimer = Mathf.Clamp(jumpInputTimer - Time.fixedDeltaTime, 0.0f, Mathf.Infinity);
        jumpTimer = Mathf.Clamp(jumpTimer - Time.fixedDeltaTime, 0.0f, Mathf.Infinity);
        FrogWalk();
        JumpUpdate();
        FrogWalkCheck();
    }

    private void FrogFaceDirection(Vector2 dir)
    {
        if (dir ==  Vector2.zero) { return; }
        transform.rotation = Quaternion.Euler(0.0f, 180.0f + VectorToAngle(dir), 0.0f);
    }

    private float VectorToAngle(Vector2 dir)
    {
        return (dir.x < 0.0f ? -1.0f : 1.0f) * Vector2.Angle(Vector2.up, dir);
    }

    private Vector2 AngleToVector(float angle)
    {
        return new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
    }

    //Called by inputhandler Move
    public void FrogWalkStart(Vector2 inputVector)
    {   
        inputDirection = inputVector;
    }

    private void FrogWalkCheck()
    {
        if (inputDirection == Vector2.zero || frogCurrWalkState != FrogWalkState.Resting) { return; }
        Vector2 camerInputDir = CameraInputDirection();
        if (frogOnGround)
        {
            FrogFaceDirection(camerInputDir);
            frogWalkDirection = new Vector3(camerInputDir.x, 0.0f, camerInputDir.y);
            frogCurrWalkState++;
            frogWalkTimer = 0.0f;
            frogWalkPastTime = 0.0f;
            PlayAnimation("Hop", frogScripObj.FrogHopAnimationSpeed);
        }
        else
        {
            //Fall Control
            //if (jumpDirection != Vector2.zero) { return; }
            FrogFaceDirection(camerInputDir);
            frogRig.velocity += (frogScripObj.FrogJumpFallControlVelocity * Time.fixedDeltaTime * new Vector3(camerInputDir.x, 0.0f, camerInputDir.y));
        }
    }

    private void FrogWalk()
    {
        if (frogCurrWalkState != FrogWalkState.Walking) { return; }
        frogWalkTimer += Time.fixedDeltaTime;
        ApplyFrogWalkMovement();
        frogWalkPastTime = frogWalkTimer;

        if (frogWalkTimer >= frogScripObj.FrogWalkTimeInterval)
        {
            frogWalkPastTime = 0.0f;
            frogWalkTimer = 0.0f;
            frogCurrWalkState++;
            Invoke(nameof(FrogWalkCoolDown), frogScripObj.FrogWalkCoolDown);
        }
    }

    private void ApplyFrogWalkMovement()
    {
        frogRig.position = InstantDistance() * frogWalkDirection + InstantHeight() * Vector3.up + frogRig.position;
    }

    private float InstantDistance()
    {
        return frogScripObj.FrogWalkDistance * ((frogWalkTimer - frogWalkPastTime) / frogScripObj.FrogWalkTimeInterval);
    }

    private float InstantHeight()
    {
        return FormulaY(FormulaX(frogWalkTimer)) - FormulaY(FormulaX(frogWalkPastTime));
    }

    private float FormulaX(float time)
    {
        return Mathf.Lerp(-frogScripObj.FrogWalkDistance * 0.5f, frogScripObj.FrogWalkDistance * 0.5f, time / frogScripObj.FrogWalkTimeInterval);
    }

    private float FormulaY(float x)
    {
        return -WalkFormulaA * x * x + WalkFormulaC;
    }

    public void FrogWalkCoolDown()
    {
        frogCurrWalkState = FrogWalkState.Resting;
    }

    public void FrogOnGround(bool tf)
    {
        frogOnGround = tf;
        if (tf)
        {
            if (jumpInputTimer <= 0.0f) { return; }
            JumpAction();
        }
        else
        {
            leaveGroundTimer = frogScripObj.FrogJumpLeaveGroundCoyoteTime;
        }
    }

    //Called by InputHandler when jump is pressed
    public void Jump()
    {
        jumpInputTimer = frogScripObj.FrogJumpInputCoyoteTime;
        if (!frogOnGround && leaveGroundTimer <= 0.0f) { return; }
        JumpAction();
    }

    private void JumpAction()
    {
        frogCurrWalkState = FrogWalkState.CoolDown;
        Invoke(nameof(FrogWalkCoolDown), frogScripObj.FrogWalkCoolDown);
        jumpInputTimer = 0.0f;
        jumpTimer = frogScripObj.FrogMaxJumpTime;
        jumpDirection = inputDirection == Vector2.zero ? Vector2.zero : CameraInputDirection();
        frogRig.useGravity = false;
        if (inputDirection != Vector2.zero)
        {
            FrogFaceDirection(CameraInputDirection());
        }
    }

    private void JumpUpdate()
    {
        if (jumpTimer > 0.0f)
        {
            frogRig.position += (Time.fixedDeltaTime * (FrogJumpVerticleVelocity() + FrogJumpForwardVelocity()));
            return;
        } 
        JumpStop();
    }

    private Vector3 FrogJumpVerticleVelocity()
    {
        return Mathf.Lerp(0.0f, frogScripObj.FrogJumpVerticleVelocity, jumpTimer / frogScripObj.FrogMaxJumpTime) * Vector3.up;
    }

    private Vector3 FrogJumpForwardVelocity()
    {
        return frogScripObj.FrogJumpForwardVelocity * new Vector3(jumpDirection.x, 0.0f, jumpDirection.y);
    }

    //Called by InputHandler when jump is released
    public void JumpStop()
    {
        if (frogRig.useGravity) { return; }
        frogRig.useGravity = true;
        frogRig.velocity = FrogJumpForwardVelocity();
        jumpDirection = Vector2.zero;
        jumpTimer = 0.0f;
    }


    //Called by InputHandler when Tongue is pressed
    public void ShootTongue()
    {

    }

    //Called by InputHandler when mouse is moved
    public void UpdateAimPosition(Vector3 cameraForward)
    {
        this.cameraForward = cameraForward;
    }

    private Vector2 CameraInputDirection()
    {
        return AngleToVector(VectorToAngle(new Vector2(cameraForward.x, cameraForward.z)) + VectorToAngle(inputDirection));
    }

    private void PlayAnimation(string parm, float playSpeed)
    {
        frogAnmt.speed = playSpeed;
        frogAnmt.SetTrigger(parm);
    }
}
