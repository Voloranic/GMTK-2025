using UnityEngine;

public class Item3 : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

    private GroundDirection groundDirection;
    private Movement playerMovement;

    private bool isFeathering;

    [SerializeField] private float featheredGravityScale = 5f;

    public void Setup(BlueprintSO so)
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = so.GetSprite();
        sprite.transform.localScale = so.GetSpriteSize();

        groundDirection = transform.root.GetComponentInChildren<GroundDirection>();
        playerMovement = transform.root.GetComponent<Movement>();

        blueprintSO = so;

        Debug.Log("Setup " + blueprintSO.GetName());
    }

    private void Update()
    {
        if (playerMovement.GetLocalVelocity().y < 0 && !groundDirection.IsGrounded())
        {
            if (!isFeathering)
            {
                isFeathering = true;
                groundDirection.ChangeGravityScale(featheredGravityScale);

                AudioManager.Instance.PlayAudio(blueprintSO.GetRandomUseAudio());
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
