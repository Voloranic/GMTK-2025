using UnityEngine;

public class Item0 : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Use Range " + blueprintSO.GetUseDistance() + " On layers " + blueprintSO.GetUsableLayers() + " With cooldown of " + blueprintSO.GetUseCooldown());
        }
    }
}
