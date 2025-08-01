using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private List<Blueprint> blueprintsInPickupDistance = new List<Blueprint>();

    private List<Transform> blueprintTransformsInPickupDistance = new List<Transform>();

    public void AddBlueprintInPickupDistance(Blueprint blueprint)
    {
        blueprintsInPickupDistance.Add(blueprint);
        blueprintTransformsInPickupDistance.Add(blueprint.transform);
    }

    public void RemoveBlueprintInPickupDistance(Blueprint blueprint)
    {
        blueprintsInPickupDistance.Remove(blueprint);
        blueprintTransformsInPickupDistance.Remove(blueprint.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (blueprintsInPickupDistance.Count <= 0)
            {
                return;
            }

            int closestBlueprintIndex = GetClosestTransformIndex(blueprintTransformsInPickupDistance);
            Blueprint closestBlueprint = blueprintsInPickupDistance[closestBlueprintIndex];
            closestBlueprint.Pickup();
        }
    }

    private int GetClosestTransformIndex(List<Transform> transforms)
    {
        float closestDistance = Vector2.Distance(transform.root.position, transforms[0].position);
        int closestDistanceIndex = 0;

        for (int i = 1; i < transforms.Count; i++)
        {
            float currentIndexDistance = Vector2.Distance(transform.root.position, transforms[i].position);

            if (currentIndexDistance < closestDistance)
            {
                closestDistance = currentIndexDistance;
                closestDistanceIndex = i;
            }
        }

        return closestDistanceIndex;
    }
}
