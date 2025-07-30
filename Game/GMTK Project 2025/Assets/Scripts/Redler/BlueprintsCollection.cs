using System.Collections.Generic;
using UnityEngine;

public class BlueprintsCollection : MonoBehaviour
{
    [SerializeField] private List<BlueprintSO> blueprintsCollection = new List<BlueprintSO>();

    public void AddBlueprintToCollection(BlueprintSO blueprint)
    {
        blueprintsCollection.Add(blueprint);
    }
}
