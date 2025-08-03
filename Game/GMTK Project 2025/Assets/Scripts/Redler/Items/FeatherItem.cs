using UnityEngine;

public class FeatherItem : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

    private GroundDirection groundDirection;
    private Movement playerMovement;

    private bool isFeathering;

    private bool canFeather = true;

    [SerializeField] private float featheredGravityScale = 5f;

    private Animator animator;

    public void Setup(BlueprintSO so)
    {
        animator = GetComponent<Animator>();
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
            if (!isFeathering && canFeather)
            {
                animator.SetBool("isUsing", true);
                isFeathering = true;
                groundDirection.ChangeGravityScale(featheredGravityScale, true);

                AudioManager.Instance.PlayAudio(blueprintSO.GetRandomUseAudio());

                Invoke(nameof(TurnOff), blueprintSO.GetUseCooldown());
            }
        }
        else
        {
            if (isFeathering)
            {
                animator.SetBool("isUsing", false);
                isFeathering = false;
                groundDirection.SetDefaultGravityScale();
            }

            if (!canFeather && groundDirection.IsGrounded())
            {
                canFeather = true;
            }
        }
    }

    private void OnDisable()
    {
        if (isFeathering)
        {
            canFeather = true;
            isFeathering = false;
            groundDirection.SetDefaultGravityScale();
        }
    }

    private void TurnOff()
    {
        if (isFeathering)
        {
            isFeathering = false;
            canFeather = false;
            groundDirection.SetDefaultGravityScale();
        }
    }
}
