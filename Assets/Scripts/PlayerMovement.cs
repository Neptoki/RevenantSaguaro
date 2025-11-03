using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference move;
    public InputActionReference fire;
    public InputActionReference jump;
    public InputActionReference sprint;
    public InputActionReference crouch;
    private Vector2 _moveDirection;
    //movement code
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    //jumping code
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    //crouch code
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    //ground check code
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    //slope code
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    //others
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    // input state
    private bool isSprinting;
    private bool isCrouching;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
    }
    private void Update()
    {
        if (move != null && move.action != null)
            _moveDirection = move.action.ReadValue<Vector2>();
        else
            _moveDirection = Vector2.zero;

        //checks if player is grounded, then handles it accordingly
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void StateHandler()
    {
        // crouch has highest priority
        if (isCrouching)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && isSprinting)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }
    //movement direction
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            //fix for bumping
            if(rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        //on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        //in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        //no gravity while on slope
        rb.useGravity = !OnSlope();
    }
    private void SpeedControl()
    {
        //limit slope speed
        if (OnSlope() && !exitingSlope)
        {
            if(rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        else
        {
            //also limits velocity by getting the max velocity so you don't go faster
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if(flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }
    private void Jump()
    {
        exitingSlope = true;
        //gets new y velocity for jump
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void OnEnable()
    {
        if (move != null && move.action != null)
            move.action.Enable();

        if (fire != null && fire.action != null)
        {
            fire.action.Enable();
            fire.action.started += Fire;
        }

        if (jump != null && jump.action != null)
        {
            jump.action.Enable();
            jump.action.started += OnJumpStarted;
        }

        if (sprint != null && sprint.action != null)
        {
            sprint.action.Enable();
            sprint.action.started += OnSprintStarted;
            sprint.action.canceled += OnSprintCanceled;
        }

        if (crouch != null && crouch.action != null)
        {
            crouch.action.Enable();
            crouch.action.started += OnCrouchStarted;
            crouch.action.canceled += OnCrouchCanceled;
        }
    }
    private void OnDisable()
    {
        if (fire != null && fire.action != null)
            fire.action.started -= Fire;

        if (jump != null && jump.action != null)
            jump.action.started -= OnJumpStarted;

        if (sprint != null && sprint.action != null)
        {
            sprint.action.started -= OnSprintStarted;
            sprint.action.canceled -= OnSprintCanceled;
        }

        if (crouch != null && crouch.action != null)
        {
            crouch.action.started -= OnCrouchStarted;
            crouch.action.canceled -= OnCrouchCanceled;
        }

        if (move != null && move.action != null)
            move.action.Disable();

        if (fire != null && fire.action != null)
            fire.action.Disable();

        if (jump != null && jump.action != null)
            jump.action.Disable();

        if (sprint != null && sprint.action != null)
            sprint.action.Disable();

        if (crouch != null && crouch.action != null)
            crouch.action.Disable();
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        Debug.Log("Fired");
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void OnSprintStarted(InputAction.CallbackContext ctx)
    {
        isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        isSprinting = false;
    }

    private void OnCrouchStarted(InputAction.CallbackContext ctx)
    {
        // perform crouch start behavior
        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void OnCrouchCanceled(InputAction.CallbackContext ctx)
    {
        // stop crouch
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
}
