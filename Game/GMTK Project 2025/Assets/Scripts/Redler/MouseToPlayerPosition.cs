using UnityEngine;

public class MouseToPlayerPosition : MonoBehaviour
{
    public static MouseToPlayerPosition Instance;

    private Camera mainCamera;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public Vector2 GetMouseWorldPosition()
    {
        Vector3 screenMousePos = Input.mousePosition;
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(screenMousePos);
        return new(worldMousePos.x, worldMousePos.y);
    }

    public RaycastHit2D ShootRayToMouse(Vector2 origin, Vector2 targetPosition, float rayLength, LayerMask hittableLayers)
    {
        Vector2 direction = (targetPosition - origin).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, hittableLayers);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.red, 1f);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * rayLength, Color.yellow, 1f);
        }

        return hit;
    }
}
