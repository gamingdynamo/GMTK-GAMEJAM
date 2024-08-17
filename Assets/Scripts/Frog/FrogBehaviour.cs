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
    private FrogGroundCheck frogGroundCheck;

    private float WalkFormulaA = 0.0f;
    private float WalkFormulaC = 0.0f;

    private Vector3 frogWalkDirection = Vector3.zero;
    private FrogWalkState frogCurrWalkState = FrogWalkState.Resting;
    private float frogWalkPastTime = 0.0f;
    private float frogWalkTimer = 0.0f;

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
        if (InputHandler.Instance.moveDirNormalized != Vector2.zero 
            && frogCurrWalkState == FrogWalkState.Resting
            && frogGroundCheck.OnGround > 0)
        {
            FrogWalkStart(InputHandler.Instance.moveDirNormalized);
        }
        if (frogCurrWalkState == FrogWalkState.Jumping)
        {
            FrogWalk();
        }
    }

    //Called by inputhandler Move
    public void FrogWalkStart(Vector2 inputVector)
    {
        frogWalkDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);
        frogCurrWalkState++;
        frogWalkTimer = 0.0f;
        frogWalkPastTime = 0.0f;
    }

    public void FrogWalk()
    {
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

    //Called by InputHandler when jump is pressed
    public void Jump()
    {

    }

    //Called by InputHandler when jump is released
    public void JumpStop()
    {

    }


    //Called by InputHandler when Tongue is pressed
    public void ShootTongue()
    {

    }

    //Called by InputHandler when mouse is moved
    public void UpdateAimPosition(Vector3 cameraForward)
    {

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
}
