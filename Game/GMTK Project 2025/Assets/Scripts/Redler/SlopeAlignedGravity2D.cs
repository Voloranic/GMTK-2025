using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlopeAlignedGravity2D : MonoBehaviour
{
    public LayerMask groundMask;
    public float rayLength = 1f;
    public float gravityStrength = 9.8f;
    public float gravityLerpSpeed = 10f;

    public float moveSpeed = 10f;

    Rigidbody2D rb;
    Vector2 currentGravity = Vector2.down * 9.8f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;       // disable default global gravity on this body
    }

    void Update()
    {
        // Read horizontal input
        float h = Input.GetAxisRaw("Horizontal");
        // Compute local-space movement direction (player�s right is local X axis)
        Vector2 desired = (Vector2)(transform.right * h) * moveSpeed;
        // Preserve vertical velocity from gravity
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, new Vector2(desired.x, rb.linearVelocity.y), 0.2f);
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundMask);
        Vector2 targetGravity;

        if (hit.collider != null)
        {
            Vector2 normal = hit.normal;
            targetGravity = -normal.normalized * gravityStrength;
        }
        else
        {
            targetGravity = -transform.up * gravityStrength;
        }

        // smooth transition
        currentGravity = Vector2.Lerp(currentGravity, targetGravity, gravityLerpSpeed * Time.fixedDeltaTime);

        rb.AddForce(currentGravity * rb.mass , ForceMode2D.Force);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)currentGravity.normalized * rayLength);
    }
}
