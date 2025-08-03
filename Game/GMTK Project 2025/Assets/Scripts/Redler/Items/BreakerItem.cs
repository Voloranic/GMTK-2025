using UnityEngine;

public class BreakerItem : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

    private Vector2 mouseWorldPos;

    private bool canUse = true;

    public void Setup(BlueprintSO so)
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = so.GetSprite();
        sprite.transform.localScale = so.GetSpriteSize();

        blueprintSO = so;

        Debug.Log("Setup " + blueprintSO.GetName());
    }

    private void Update()
    {
        if (!canUse)
            return; 

        mouseWorldPos = MouseToPlayerPosition.Instance.GetMouseWorldPosition();
        Debug.DrawLine(transform.position, mouseWorldPos, Color.green);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Use Range " + blueprintSO.GetUseDistance() + " On layers " + blueprintSO.GetUsableLayers() + " With cooldown of " + blueprintSO.GetUseCooldown());
            Use();
            canUse = false;
            Invoke(nameof(CanUse), blueprintSO.GetUseCooldown());
        }
    }

    private void Use()
    {
        //Do Animation

        AudioManager.Instance.PlayAudio(blueprintSO.GetRandomUseAudio());

        RaycastHit2D useRay = MouseToPlayerPosition.Instance.ShootRayToMouse(transform.root.position, mouseWorldPos, blueprintSO.GetUseDistance(), blueprintSO.GetUsableLayers());

        if (useRay && useRay.transform.CompareTag(blueprintSO.GetUsableTag()))
        {
            DestroyBreakable(useRay);
        }
    }

    private void DestroyBreakable(RaycastHit2D useRay)
    {
        GameObject hittedObject = useRay.collider.gameObject;
        Destroy(hittedObject);
    }

    private void CanUse()
    {
        canUse = true;
    }
}
