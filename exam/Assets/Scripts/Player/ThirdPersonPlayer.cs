using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    [Header("Mobile Controls")]
    public MobileJoystick joystick;

    private CharacterController characterController;

    private Vector3 velocity;
    private bool jumpRequested;
    private float movementAmount;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
        UpdateAnimations();

        // Temporary keyboard jump for testing.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void HandleMovement()
    {
        // Keyboard input.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 keyboardInput =
            new Vector2(horizontal, vertical);

        // Mobile joystick input.
        Vector2 joystickInput = Vector2.zero;

        if (joystick != null)
        {
            joystickInput = joystick.InputDirection;
        }

        // Use joystick when it is being moved.
        // Otherwise use keyboard.
        Vector2 input;

        if (joystickInput.magnitude > 0.1f)
        {
            input = joystickInput;
        }
        else
        {
            input = keyboardInput;
        }

        input = Vector2.ClampMagnitude(input, 1f);

        // Store movement amount for animations.
        movementAmount = input.magnitude;

        if (input.magnitude < 0.01f)
            return;

        // Movement relative to camera.
        Vector3 cameraForward =
            cameraTransform.forward;

        Vector3 cameraRight =
            cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection =
            cameraForward * input.y +
            cameraRight * input.x;

        // Move player.
        characterController.Move(
            moveDirection *
            moveSpeed *
            Time.deltaTime
        );

        // Rotate player toward movement direction.
        Quaternion targetRotation =
            Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void HandleGravity()
    {
        if (characterController.isGrounded &&
            velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (jumpRequested &&
            characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );

            jumpRequested = false;
        }

        velocity.y +=
            gravity * Time.deltaTime;

        characterController.Move(
            velocity * Time.deltaTime
        );
    }

    void UpdateAnimations()
    {
        if (animator == null)
            return;

        animator.SetFloat(
            "Speed",
            movementAmount
        );

        animator.SetBool(
            "IsJumping",
            !characterController.isGrounded
        );
    }

    public void Jump()
    {
        jumpRequested = true;
    }

    public void Respawn()
    {
        if (CheckpointManager.currentCheckpoint == null)
        {
            Debug.Log(
                "No checkpoint set! Returning to starting position."
            );

            return;
        }

        characterController.enabled = false;

        transform.position =
            CheckpointManager.currentCheckpoint.position;

        velocity = Vector3.zero;
        jumpRequested = false;

        characterController.enabled = true;

        Debug.Log("Player Respawned!");
    }
}