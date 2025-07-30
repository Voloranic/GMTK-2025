using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private float horizontalInput;

    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float jumpForce = 2.5f;
    [SerializeField] private float airborneMoveSpeedDebuff = 0.7f; //Decreases the walking speed in the air by percentage

    [SerializeField] private float defaultGravity = 6.5f;
    [SerializeField] private float fallingGravity = 7.8f;

    private bool isGrounded;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float bufferTime = 0.2f;
    private float coyoteTimeCounter = 0f;
    private float bufferTimeCounter = 0f;

    private bool isFacingRight = true;

    public enum States
    {
        Idle,
        Walking,
        Airborne
    }
    private States currentState = States.Idle;
    private bool hasState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        GroundConfig();

        SpriteConfig();

        DetermineState();

        GravityConfig();
    }

    private void GravityConfig()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = fallingGravity;
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }

        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -rb.gravityScale, float.MaxValue);
    }

    #region State Machine

    private void DetermineState()
    {
        if (hasState)
        {
            return;
        }

        if (!isGrounded)
        {
            if (currentState != States.Airborne)
            {
                StartAirborne();
                
            }
            return;
        }
        else
        {
            if (horizontalInput != 0)
            {
                if (currentState != States.Walking)
                {
                    StartWalking();
                }
            }
            else
            {
                if (currentState != States.Idle)
                {
                    StartIdle();
                }
            }
        }
    }

    private void StartIdle()
    {
        Debug.Log("Idle");

        hasState = true;
        currentState = States.Idle;
        //Start animation
    }

    private void StartWalking()
    {
        Debug.Log("Walking");
        hasState = true;
        currentState = States.Walking;
        //Start animation
    }

    private void StartAirborne()
    {
        Debug.Log("Airborne");
        hasState = true;
        currentState = States.Airborne;
        //Start animation
    }

    private void FixedUpdate()
    {
        MovementConfig();
    }

    private void MovementConfig()
    {
        switch (currentState)
        {
            case States.Airborne:
                DoAirborne();
                break;
            case States.Walking:
                DoWalking();
                break;
            case States.Idle:
                DoIdle();
                break;
        }
    }

    private void DoIdle()
    {
        //If certain conditions are met the script will exit this state and find a new one
        if (!isGrounded)
        {
            hasState = false;
            return;
        }
        if (horizontalInput != 0)
        {
            hasState = false;
            return;
        }
    }

    private void DoWalking()
    {
        float moveSpeed = walkingSpeed;

        RigidbodyMove(moveSpeed, horizontalInput);

        //If certain conditions are met the script will exit this state and find a new one
        if (!isGrounded)
        {
            hasState = false;
            return;
        }
        if (horizontalInput == 0)
        {
            hasState = false;
            return;
        }
    }

    private void DoAirborne()
    {
        float moveSpeed = walkingSpeed - (walkingSpeed * airborneMoveSpeedDebuff);

        RigidbodyMove(moveSpeed, horizontalInput);

        //If certain conditions are met the script will exit this state and find a new one
        if (isGrounded)
        {
            hasState = false;
            return;
        }
    }

    #endregion

    private void RigidbodyMove(float moveSpeed, float input)
    {
        rb.linearVelocityX = input * moveSpeed;
    }

    private void SpriteConfig()
    {
        if (horizontalInput < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (horizontalInput > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void GroundConfig()
    {
        bool lastIsGrounded = isGrounded;

        isGrounded = Physics2D.CircleCast(groundCheckTransform.position, groundCheckRadius, Vector2.down, 0f, groundLayer);

        coyoteTimeCounter -= Time.deltaTime;
        bufferTimeCounter -= Time.deltaTime;

        //The player disconnected from the ground
        if (lastIsGrounded && !isGrounded)
        {
            //The player fell from a wedge
            if (rb.linearVelocityY <= 0f)
            {
                coyoteTimeCounter = coyoteTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //The player clicked the space button while falling
            if (!isGrounded && rb.linearVelocityY < 0f)
            {
                //The player clicked the jump button a bit after he fell from a wedge
                if (coyoteTimeCounter > 0)
                {
                    coyoteTimeCounter = 0;
                    Jump();
                }
                //The player clicked the jump button a bit before he reached the ground
                else
                {
                    bufferTimeCounter = bufferTime;
                }
            }
            else if (isGrounded)
            {
                Jump();
            }
        }

        //The player reached the ground
        if (!lastIsGrounded && isGrounded)
        {
            if (bufferTimeCounter > 0)
            {
                bufferTimeCounter = 0;
                Jump();
            }
        }
    }

    private void Jump()
    {
        rb.linearVelocityY = 0f;
        rb.AddForceY(jumpForce, ForceMode2D.Impulse);
    }
}
