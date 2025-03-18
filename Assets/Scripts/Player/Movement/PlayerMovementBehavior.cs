using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour
{
    [Header("MovementSettings")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxMoveSpeed;
    [SerializeField] public float groundDrag;

    [Header("JumpSettings")]
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airSpeedCorrection;

    [Header("Pre-Required For Script")]
    [SerializeField] public Transform orientation;

    [Header("Ground Check")]
    [SerializeField] public float PlayerHeight;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] bool grounded;
    [SerializeField] bool readyToJump;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpkey = KeyCode.Space;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //checks for the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, whatIsGround);

        //controls the maxspeed of the player
        SpeedControl();

        //handles drag if the plyaer is on the ground
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        //calls for the player's input
        MyInput();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Grabs the inputs for the player
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Checks to see if the player is pressing the jump button and is ready to jump
        if (Input.GetKey(jumpkey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded == true) 
        {
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * airSpeedCorrection), ForceMode.Force);
        }
        
    }

    private void Jump()
    {
        //reset y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce , ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //checks the current player's velocity and compares it to the maxmovement speed to slow them down to it
        if (flatVel.magnitude > maxMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxMoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
