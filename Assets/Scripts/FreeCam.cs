using UnityEngine;

public class FreeCam : MonoBehaviour
{
    private Transform cameraTrans;

    public float movementSpeed = 10f;
    public float fastSpeedMultiplier = 5f;
    public float mouseSensitivity = 2f;

    private float yaw = 0f;
    private float pitch = 0f;

    private void Start()
    {
        cameraTrans = Camera.main.transform;
    }

    void Update()
    {
        // Cursor Locking
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
        /*else if (Input.GetMouseButtonUp(0))
        {
            UnlockCursor();
        }*/

        if (Cursor.lockState == CursorLockMode.Locked || Cursor.lockState == CursorLockMode.Confined)
        {
            // Mouse look
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Apply rotation
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        if (FrogBehaviour.Instance != null) 
        {
            FrogBehaviour.Instance.UpdateAimPosition(cameraTrans.forward);
            transform.position = FrogBehaviour.Instance.transform.position;
        }
           
        // Movement (always active, even if cursor is not locked)
            /*Vector3 movement = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) movement += transform.forward;
            if (Input.GetKey(KeyCode.S)) movement -= transform.forward;
            if (Input.GetKey(KeyCode.A)) movement -= transform.right;
            if (Input.GetKey(KeyCode.D)) movement += transform.right;
            if (Input.GetKey(KeyCode.Space)) movement += transform.up;
            if (Input.GetKey(KeyCode.LeftControl)) movement -= transform.up;*/

            // Apply speed and move
            //float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? movementSpeed * fastSpeedMultiplier : movementSpeed;
            //transform.position += movement * currentSpeed * Time.deltaTime;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
