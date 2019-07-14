using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private CharacterController controller;

    // Movement variables
    [SerializeField] private float moveSpeed;
    private bool isJumping;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 forwardMove = transform.forward * verticalInput;
        Vector3 sideMove = transform.right * horizontalInput;

        controller.SimpleMove(Vector3.ClampMagnitude(forwardMove + sideMove, 1.0f) * moveSpeed);

        JumpInput();
    }

    private void JumpInput()
    {
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        controller.slopeLimit = 90f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = 45f;
        isJumping = false;
    }
}
