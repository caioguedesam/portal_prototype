using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

    //private PlayerControls controls;
    private CharacterController controller;

    // Movement variables
    [SerializeField] private float moveSpeed;
    private bool isJumping;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;

    private Vector3 forwardMove;
    private Vector3 upMove;
    private Vector3 sideMove;
    private Vector3 moveDirection;

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

        forwardMove = transform.forward * verticalInput;
        sideMove = transform.right * horizontalInput;

        moveDirection = Vector3.ClampMagnitude(forwardMove + sideMove, 1.0f) * moveSpeed;

        if(!isJumping)
           controller.SimpleMove(moveDirection);

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

            upMove = (Vector3.up * jumpForce * jumpMultiplier) + moveDirection;

            controller.Move(upMove * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = 45f;
        isJumping = false;
    }
    
}
