using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Transform orientation; 
    private Vector3 moveDirection;
    private Rigidbody rb;

    private float horizontalInput;
    private float verticalInput;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float jumpCooldown;
    
    [Header("Grounded")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool readyToJump = true;
    private bool isGrounded;

    public bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate() {
        if (!canMove) return;
        movePlayer();
    }

    private void Update()
    {
        if (!canMove) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded) {
            Debug.Log("Jump");
            readyToJump = false;
            Jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }

        speedControl();

        if (isGrounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = airDrag;
        }
    }

    private void movePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (isGrounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        } else {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void speedControl() {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (currentVelocity.magnitude > moveSpeed) {
            Vector3 newVelocity = currentVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump() {
        readyToJump = true;
    }

    public bool getGrounded() {
        return isGrounded;
    }
}
