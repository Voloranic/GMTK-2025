using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontalInput;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    GroundDirection groundDirectionScript;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDirectionScript = GetComponentInChildren<GroundDirection>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && groundDirectionScript.IsGrounded())
        {
            Jump();
        }

        Move();
    }
    private void Move()
    {
        Vector2 speedForce = horizontalInput * transform.right * speed;

        rb.AddForce(speedForce, ForceMode2D.Force);

        ClampSpeed();
        
    }
    private void ClampSpeed()
    {
        // Convert current world velocity to local space (relative to player rotation)
        Vector2 worldVelocity = rb.linearVelocity;
        Vector2 localVelocity = transform.InverseTransformDirection(worldVelocity);

        // Clamp only the local X (horizontal) speed
        if (Mathf.Abs(localVelocity.x) > speed)
        {
            localVelocity.x = Mathf.Sign(localVelocity.x) * speed;

            // Convert the clamped velocity back to world space and apply it
            rb.linearVelocity = transform.TransformDirection(localVelocity);
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

}

