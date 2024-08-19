using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : MonoBehaviour
{
    public static FrogBehaviour Instance { get; private set; }

    [SerializeField]
    private FrogScriptableObject frogScripObj;
    public FrogScriptableObject FrogScripObj { get { return frogScripObj; } }
    [SerializeField]
    private Rigidbody frogRig;
    [SerializeField]
    private Animator frogAnmt;
    [SerializeField]
    private TongueBehaviour frogTongue;
    [SerializeField]
    private FrogTongueAssist aimAssist;

    private Camera cameraMain;
    public Camera CameraMain { 
        get { 
            if (cameraMain == null)
            {
                cameraMain = Camera.main;
            }
            return cameraMain; 
        } 
    }

    private float WalkFormulaA = 0.0f;
    private float WalkFormulaC = 0.0f;

    private Vector2 inputDirection = Vector2.zero;
    private Vector3 frogWalkDirection = Vector3.zero;
    private FrogWalkState frogCurrWalkState = FrogWalkState.Resting;
    private float frogWalkPastTime = 0.0f;
    private float frogWalkTimer = 0.0f;

    private bool frogOnGround = false;
    private float leaveGroundTimer = 0.0f;
    private float jumpInputTimer = 0.0f;
    private float jumpTimer = 0.0f;
    private Vector2 jumpDirection = Vector2.zero;

    private bool shootingTongue = false;

    private int frogScaleLevel = 0;
    private int frogJumpLevel = 0;
    private int FrogTongueLevel = 0;

    private void Start()
    {
        //Put at a better place later
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        ////////////////////////////////////
        
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
        aimAssist.InitBoxColliders();
        aimAssist.SetAimAssist(GetTongueLength() / frogScripObj.AimAssistColliderNumber / 2.0f);
    }

    private void FixedUpdate()
    {
        aimAssist.SetAimAssistRotation((AimPosition() - transform.position).normalized);
        aimAssist.transform.position = transform.position + FrogScripObj.AimAssistYOffset * Vector3.up;
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
        transform.rotation = Quaternion.Euler(0.0f, 180.0f + GameManager.VectorToAngle(dir), 0.0f);
    }

    //Called by inputhandler Move
    public void FrogWalkStart(Vector2 inputVector)
    {   
        inputDirection = inputVector;
    }

    private void FrogWalkCheck()
    {
        if (inputDirection == Vector2.zero || frogCurrWalkState != FrogWalkState.Resting || shootingTongue) { return; }
        Vector2 camerInputDir = CameraInputDirection();
        if (frogOnGround)
        {
            FrogFaceDirection(camerInputDir);
            frogWalkDirection = new Vector3(camerInputDir.x, 0.0f, camerInputDir.y);
            frogCurrWalkState++;
            frogWalkTimer = 0.0f;
            frogWalkPastTime = 0.0f;
            TriggerAnimation("Hop", frogScripObj.FrogHopAnimationSpeed);
        }
        else
        {
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
            frogWalkDirection = Vector3.zero;
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
        frogWalkDirection = Vector3.zero;
        jumpInputTimer = 0.0f;
        jumpTimer = GetJumpTime();
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
        return Mathf.Lerp(0.0f, GetJumpYLeveledVelocity(), jumpTimer / frogScripObj.FrogMaxJumpTime) * Vector3.up;
    }

    private Vector3 FrogJumpForwardVelocity()
    {
        return GetJumpXZLeveledVelocity() * new Vector3(jumpDirection.x, 0.0f, jumpDirection.y);
    }

    //Called by InputHandler when jump is released
    public void JumpStop()
    {
        if (frogRig.useGravity) { return; }
        frogRig.useGravity = true;
        frogRig.velocity = new Vector3(frogRig.velocity.x, 0.0f, frogRig.velocity.z);
        if (jumpDirection != Vector2.zero)
        {
            frogRig.velocity += FrogJumpForwardVelocity();
        }
        jumpDirection = Vector2.zero;
        jumpTimer = 0.0f;
    }


    //Called by InputHandler when Tongue is pressed
    public void ShootTongue()
    {
        if (frogWalkDirection != Vector3.zero || shootingTongue) { return; }
        FrogFaceDirection(new Vector2(CameraMain.transform.forward.x, CameraMain.transform.forward.z));
        shootingTongue = true;
        frogAnmt.SetBool("Shooting", true);
        Invoke(nameof(TongueShotFinish), (frogScripObj.FrogTongueShootTime + frogScripObj.FrogTongueHoldTime + frogScripObj.FrogTongueRetrieveTime));
        Tonguable target = aimAssist.LockInTarget();
        if (target != null)
        {
            //target.GotTongued();
            frogTongue.ShootTongue(target);
        }
        else
        {
            frogTongue.ShootTongue(AimPosition());
        }
    }

    public Vector3 AimPosition()
    {
        return FrogScripObj.FrogTongueAimDistance * CameraMain.transform.forward + CameraMain.transform.position;
    }

    public void TongueShotFinish()
    {
        shootingTongue = false;
        frogAnmt.SetBool("Shooting", false);
    }

    private Vector2 CameraInputDirection()
    {
        return GameManager.AngleToVector(GameManager.VectorToAngle(new Vector2(CameraMain.transform.forward.x, CameraMain.transform.forward.z)) + GameManager.VectorToAngle(inputDirection));
    }

    public void UpgradeFrog(FlyUpgradeType type)
    {
        switch (type)
        {
            case FlyUpgradeType.AllType:
                UpgradeFrogAll();
                break;
            case FlyUpgradeType.Scaling:
                    UpgradeScale();
                break;
            case FlyUpgradeType.Jumping:
                    UpgradeJump();
                break;
            case FlyUpgradeType.Tonguing:
                    UpgradeTongue();
                break;
            default:
                Debug.Log("ERROR: Wait what?? what are you trying to upgrade??");
                break;
        }

    }

    private void UpgradeScale()
    {
        frogScaleLevel++;
        transform.localScale = (1.0f + frogScaleLevel * frogScripObj.FrogScaleIncreaseAmount) * Vector3.one;
    }

    private void UpgradeJump()
    {
        frogJumpLevel++;
    }

    public void UpgradeTongue()
    {
        FrogTongueLevel++;
        aimAssist.SetAimAssist(GetTongueLength() / frogScripObj.AimAssistColliderNumber / 2.0f);
    }

    private void UpgradeFrogAll()
    {
        UpgradeScale();
        UpgradeJump();
        UpgradeTongue();
    }

    public float GetJumpTime()
    {
        return frogScripObj.FrogMaxJumpTime + frogScripObj.FrogJumpTimeIncreaseAmount * frogJumpLevel;
    }

    public float GetJumpYLeveledVelocity()
    {
        return frogScripObj.FrogJumpVerticleVelocity + frogScripObj.FrogJumpYVelocityIncreaseAmount * frogJumpLevel;
    }

    public float GetJumpXZLeveledVelocity()
    {
        return frogScripObj.FrogJumpForwardVelocity + frogScripObj.FrogJumpXZVelocityIncreaseAmount * frogJumpLevel;
    }


    public float GetTongueLength()
    {
        return frogScripObj.FrogTongueMaxLength + frogScripObj.FrogTongueLengthIncreaseAmount * FrogTongueLevel;
    }


    private void TriggerAnimation(string parm, float playSpeed)
    {
        frogAnmt.speed = playSpeed;
        frogAnmt.SetTrigger(parm);
    }
}
