using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontalInput;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    GroundDirection groundDirectionScript;

    Transform spriteTransform;

    private bool isFacingRight = true;
    private bool move;
    private bool jump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDirectionScript = GetComponentInChildren<GroundDirection>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform.parent;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        FlipSprite();

        if (Input.GetKeyDown(KeyCode.Space) && groundDirectionScript.IsGrounded())
        {
            Jump();
        }

    }
    void FixedUpdate()
    {
        Move();
    }

    public bool GetIsFacingRight()
    {
        return isFacingRight;
    }

    private void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0)
        {
            isFacingRight = false;
            spriteTransform.transform.localEulerAngles = new(0f, 180f, 0f);
        }
        if (!isFacingRight && horizontalInput > 0)
        {
            isFacingRight = true;
            spriteTransform.transform.localEulerAngles = new(0f, 0f, 0f);
        }
    }

    private void Move()
    {
        //float directionMultiplier = isFacingRight ? 1f : -1f;
        Vector2 speedForce = horizontalInput * speed * transform.right / Time.deltaTime;

        rb.AddForce(speedForce, ForceMode2D.Force);
    }
   
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public Vector2 GetLocalVelocity()
    {
        Vector2 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        return localVelocity;
    }

}

