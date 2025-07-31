using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontalInput;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] GroundDirection GD;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && GD.IsGrounded())
        {
            Jump();
        }
        Move();
    }
    private void Move()
    {
        switch (horizontalInput)
        {
            case -1:
                rb.AddForce(-transform.right * speed, ForceMode2D.Force);
                break;
            case 1:
                rb.AddForce(transform.right * speed,ForceMode2D.Force);
                break;

        }
        Clamp();
        
    }
    private void Clamp()
    {
        // Get the current velocity from the Rigidbody2D.
        Vector2 currentVelocity = rb.linearVelocity;

        // Check if the magnitude (total speed) of the velocity exceeds the max speed.
        if (currentVelocity.magnitude > speed)
        {
            // Normalize the vector to get its direction, then multiply by maxSpeed.
            // This scales the speed down to the max limit while preserving the direction.
            rb.linearVelocity = currentVelocity.normalized * speed;
        }
    }
    
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

}

