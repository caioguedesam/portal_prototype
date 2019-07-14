using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls a first person camera's rotation using mouse axis
// Still has to be extended for joystick controls, using right analog stick
// Heavily inspired by this tutorial: https://www.youtube.com/watch?v=n-KX8AeGK7E

// Caio Guedes de Azevedo Mota, 07/2019

public class PlayerLook : MonoBehaviour
{
    // Input variables
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [Range(1f, 100f)]
    [SerializeField] private float mouseSensitivity;
    // Clamp variable to control adequate clamping
    private float xAxisClamp;

    // Player body reference
    [SerializeField] private Transform playerBody;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;
    }

    void Update()
    {
        CameraRotation();
    }

    // Locks cursor in center by simply setting cursor lock state to locked
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Controls X axis camera rotation (Up/Down).
    // Y axis rotation (Left/Right) is done by using forward vector
    // in player movement rather than camera movement, so that the player's
    // body rotates along the camera.
    private void CameraRotation()
    {
        // Getting input
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity/5f;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity/5f;

        // Controlling clamp by limiting rotation, xAxisClamp stores amount rotated
        xAxisClamp += mouseY;
        // If the total rotation exceeds upper bound (90 deg),
        // set the rotation input to 0 so it won't rotate
        // and call Clamp function to avoid buggy behavior
        if(xAxisClamp > 90f)
        {
            xAxisClamp = 90f;
            mouseY = 0.0f;
            ClampXAxisRotation(270f);
        }
        // Same as above but about lower bound (-90 deg)
        else if(xAxisClamp < -90f)
        {
            xAxisClamp = -90f;
            mouseY = 0.0f;
            ClampXAxisRotation(90f);
        }

        // Rotate based on input
        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // Directly clamps x axis rotation to a specific value
    // This stops camera from overshooting the determined clamp
    // by altering euler angles directly.
    private void ClampXAxisRotation(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

}
