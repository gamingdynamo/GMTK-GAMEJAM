using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    PlayerInputs playerInputs;
    public static InputHandler Instance;
    /// <summary>
    /// if jump is being pressed
    /// </summary>
    public bool Jump;
    /// <summary>
    /// MouseButton0
    /// </summary>
    public bool TongueOut;
    public Vector2 moveDirNormalized;
    /// <summary>
    /// ScreenSpace
    /// </summary>
    public Vector2 mousePosition;
    // Start is called before the first frame update
    void Awake()
    {   
        if(Instance == null)
        {
            Instance = this;
        }
        playerInputs = new PlayerInputs();
        playerInputs.Player.Jump.started += context => { Jump = true; };
        playerInputs.Player.Jump.canceled += context => { Jump = false; };
        playerInputs.Player.Tongue.started += context => { TongueOut = true;  };
        playerInputs.Player.Tongue.canceled += context => { TongueOut = false; };
        playerInputs.Player.Move.performed += context => Move(context);
        playerInputs.Player.Move.canceled += context => Move(context);



    }

    private void Move(InputAction.CallbackContext context)
    {
        moveDirNormalized = context.ReadValue<Vector2>();
        moveDirNormalized.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
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
