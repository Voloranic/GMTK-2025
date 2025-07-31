using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlopeStickyPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravityForce = 30f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 groundNormal = Vector2.up;
    private bool isGrounded;

    private bool isFacingRight = true;
    [SerializeField] private Transform spriteTransform;

    private float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable built-in gravity
    }

    void FixedUpdate()
    {
        UpdateGroundInfo();
        ApplyGravity();
        MoveAlongSurface();
        FlipSpriteByMovementDirection();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FlipSpriteByMovementDirection()
    {
        // Get velocity along the player's right direction
        float lateralSpeed = Vector2.Dot(rb.linearVelocity, transform.right);

        // If not moving significantly, don�t change facing
        if (Mathf.Abs(lateralSpeed) < 0.01f)
            return;

        // Flip the local X scale of the sprite
        Vector3 scale = spriteTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * (lateralSpeed < 0 ? -1 : 1);
        spriteTransform.localScale = scale;
    }

    void UpdateGroundInfo()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.2f, -transform.up, groundCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            groundNormal = hit.normal;
            isGrounded = true;

            // Smooth rotation to align with slope
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.2f);
        }
        else
        {
            isGrounded = false;
            // still keep using the last known groundNormal to pull player back
        }
    }

    void ApplyGravity()
    {
        Vector2 gravityDirection = -groundNormal.normalized;
        float distanceFactor = isGrounded ? 1f : 1.5f; // stronger pull in air
        rb.AddForce(gravityDirection * gravityForce * distanceFactor);
    }

    void MoveAlongSurface()
    {
        input = Input.GetAxis("Horizontal");

        // Tangent to the ground (perpendicular to normal)
        Vector2 moveDirection = Vector2.Perpendicular(groundNormal).normalized;

        // Flip direction based on input
        if (Vector2.Dot(moveDirection, Vector2.right) < 0)
            moveDirection = -moveDirection;

        rb.linearVelocity = moveDirection * input * moveSpeed + Vector2.Dot(rb.linearVelocity, groundNormal) * groundNormal;
    }

    void Jump()
    {
        // Cancel velocity along ground normal to get consistent jump height
        rb.linearVelocity -= Vector2.Dot(rb.linearVelocity, groundNormal) * groundNormal;

        // Apply impulse in the jump direction (away from ground)
        rb.AddForce(groundNormal * jumpForce, ForceMode2D.Impulse);
    }
}
