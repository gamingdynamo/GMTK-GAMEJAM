using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    PlayerInputs playerInputs;
    public static InputHandler Instance;
    /*/// <summary>
    /// if jump is being pressed
    /// </summary>
    /// <summary>
    /// MouseButton0
    /// </summary>
    public bool TongueOut;
    public Vector2 moveDirNormalized;
    /// <summary>
    /// ScreenSpace
    /// </summary>
    /// */
    public Vector2 mousePosition;
    void Awake()
    {   
        if(Instance == null)
        {
            Instance = this;
        }
        playerInputs = new PlayerInputs();
        playerInputs.Player.Jump.started += context => Jump();
        playerInputs.Player.Jump.canceled += context => JumpCancel();
        playerInputs.Player.Tongue.started += context => ShootTongue();
        //If we want grapple or pull with tongue maybe we need this. I Just comment it for now.
        //playerInputs.Player.Tongue.canceled += context => { TongueOut = false; };
        playerInputs.Player.Move.performed += context => Move(context);
        playerInputs.Player.Move.canceled += context => Move(context);



    }

    private void Move(InputAction.CallbackContext context)
    {
        FrogBehaviour.Instance.FrogWalkStart(context.ReadValue<Vector2>().normalized);
    }

    private void Jump()
    {
        FrogBehaviour.Instance.Jump();
    }

    private void JumpCancel()
    {
        FrogBehaviour.Instance.JumpStop();
    }

    private void ShootTongue()
    {
        FrogBehaviour.Instance.ShootTongue();
    }


    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        //Update FrogTongue Aimming Direction
        //FrogBehaviour.Instance.UpdateAimPosition(CameraForwardDirection)
    }


    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();

    }

    /// <summary>
    /// For disabling the player controller
    /// </summary>
    /// <param name="isActive"></param>
    public void ControlsActive(bool isActive = true)
    {
        if (isActive) { 
            playerInputs.Enable(); 
        }
        playerInputs.Disable();
    }
}
