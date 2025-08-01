using UnityEngine;

public class Item1 : MonoBehaviour
{
    private BlueprintSO blueprintSO;

    private SpriteRenderer sprite;

    private Vector2 mouseWorldPos;

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
            int moveDirection = transform.root.GetComponent<Movement>().GetIsFacingRight() ? 1 : -1;

            Vector2 bridgeStartPosition = transform.root.position + (transform.root.right * moveDirection);

            CreateGroundBetweenPoints(bridgeStartPosition, useRay.point, blueprintSO.GetCustomPrefab());
        }
    }

    private void CreateGroundBetweenPoints(Vector2 pointA, Vector2 pointB, GameObject groundPrefab)
    {
        // Calculate the center position between pointA and pointB
        Vector2 centerPos = (pointA + pointB) / 2f;

        // Calculate the direction and distance
        Vector2 direction = pointB - pointA;
        float distance = direction.magnitude;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate the ground object
        GameObject ground = Instantiate(groundPrefab, centerPos, Quaternion.identity);

        // Set the scale based on the distance (assuming original ground width is 1 unit)
        ground.transform.right = direction.normalized; // Rotate toward direction
        ground.transform.localScale = new Vector3(distance, ground.transform.localScale.y, 1f);
    }
}
