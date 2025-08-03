using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Rigidbody2D PlayerRB;
    [SerializeField] float moveSpeed = 5f;

    int direction = 1;

    Vector2 localVelocity;

    public enum MoveAxis
    {
        X, Y
    }
    [SerializeField] MoveAxis moveAxis = MoveAxis.X;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        if (moveAxis == MoveAxis.X)
        {
            localVelocity.x = moveSpeed * direction;
        }
        else
        {
            localVelocity.y = moveSpeed * direction;
        }

        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SwitchMovement"))
        {
            print("Gyattttttttt");
            direction *= -1;
            UpdateVelocity();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (moveAxis == MoveAxis.X)
            {
                PlayerRB.linearVelocityX += rb.linearVelocityX;
            }
            else
            {
                PlayerRB.linearVelocityY += rb.linearVelocityY;

            }
        }
    }
}
