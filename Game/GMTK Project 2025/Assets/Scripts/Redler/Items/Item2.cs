using UnityEngine;

public class Item2 : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

    private Vector2 mouseWorldPos;

    [SerializeField] private float wallAngleThreshold = 70f;

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
        mouseWorldPos = MouseToPlayerPosition.Instance.GetMouseWorldPosition();
        Debug.DrawLine(transform.position, mouseWorldPos, Color.green);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Use Range " + blueprintSO.GetUseDistance() + " On layers " + blueprintSO.GetUsableLayers() + " With cooldown of " + blueprintSO.GetUseCooldown());
            Use();
        }
    }

    private void Use()
    {
        //Do Animation

        RaycastHit2D useRay = MouseToPlayerPosition.Instance.ShootRayToMouse(transform.root.position, mouseWorldPos, blueprintSO.GetUseDistance(), blueprintSO.GetUsableLayers());

        if (useRay)
        {
            PlaceFloorOnWall(useRay);
        }
    }

    private void PlaceFloorOnWall(RaycastHit2D hit)
    {
        Vector2 normal = hit.normal;
        float angleFromVertical = Vector2.Angle(normal, Vector2.up);

        // Check if it's a "wall" (angle close to 90 degrees from up)
        if (Mathf.Abs(angleFromVertical - 90f) <= wallAngleThreshold)
        {
            // Determine placement offset
            float prefabWidth = blueprintSO.GetCustomPrefab().GetComponent<SpriteRenderer>().bounds.size.x;

            Vector2 placementPos = hit.point;

            // Determine which side the hit wall is on relative to the origin
            bool hitOnLeft = hit.point.x < transform.root.position.x;

            // Offset to the right if wall is left, and to the left if wall is right
            if (hitOnLeft)
            {
                placementPos.x += prefabWidth / 2f;
            }
            else
            {
                placementPos.x -= prefabWidth / 2f;
            }

            // Align prefab horizontally
            Instantiate(blueprintSO.GetCustomPrefab(), placementPos, Quaternion.identity);
        }
    }
}
