using UnityEngine;

public class Item3 : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

    private GroundDirection groundDirection;
    private Rigidbody2D playerRb;

    private bool isFeathering;

    [SerializeField] private float featheredGravityScale = 5f;

    public void Setup(BlueprintSO so)
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = so.GetSprite();
        sprite.transform.localScale = so.GetSpriteSize();

        groundDirection = transform.root.GetComponentInChildren<GroundDirection>();
        playerRb = transform.root.GetComponent<Rigidbody2D>();

        blueprintSO = so;

        Debug.Log("Setup " + blueprintSO.GetName());
    }

    private void Update()
    {
        if (playerRb.linearVelocityY < 0)
        {
            if (!isFeathering)
            {
                isFeathering = true;
                groundDirection.ChangeGravityScale(featheredGravityScale);
            }
        }
        else
        {
            if (isFeathering)
            {
                isFeathering = false;
                groundDirection.SetDefaultGravityScale();
            }
        }
    }
}
