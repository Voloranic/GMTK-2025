using System.Collections.Generic;
using UnityEngine;

public class BlueprintsCollection : MonoBehaviour
{
    [SerializeField] private Item0 item0Child;
    [SerializeField] private Item1 item1Child;
    [SerializeField] private Item2 item2Child;
    [SerializeField] private Item3 item3Child;

    private List<BlueprintSO> blueprintsCollection = new List<BlueprintSO>();

    [SerializeField] private KeyCode[] blueprintsKeys = new KeyCode[0];

    private BlueprintSO equipedBlueprintSO;

    public void AddBlueprintToCollection(BlueprintSO blueprint)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        blueprintsCollection.Add(blueprint);
        GameCanvas.Instance.AddItemToInventory(blueprint.GetId());

        switch (blueprint.GetId())
        {
            case 0:
                item0Child.Setup(blueprint);
                item0Child.gameObject.SetActive(true);
                break;
            case 1:
                item1Child.Setup(blueprint);
                item1Child.gameObject.SetActive(true);
                break;
            case 2:
                item2Child.Setup(blueprint);
                item2Child.gameObject.SetActive(true);
                break;
            case 3:
                item3Child.Setup(blueprint);
                item3Child.gameObject.SetActive(true);
                break;
        }

        GameCanvas.Instance.BoldItem(blueprint.GetId());
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < blueprintsCollection.Count; i++)
        {
            KeyCode key = blueprintsKeys[i];

            if (Input.GetKeyDown(key))
            {
                //Deactivate all active items
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).gameObject.SetActive(false);
                }

                equipedBlueprintSO = blueprintsCollection[i];

                //Activate the clicked item based on its id
                switch (equipedBlueprintSO.GetId())
                {
                    case 0:
                        item0Child.gameObject.SetActive(true);
                        break;
                    case 1:
                        item1Child.gameObject.SetActive(true);
                        break;
                    case 2:
                        item2Child.gameObject.SetActive(true);
                        break;
                    case 3:
                        item3Child.gameObject.SetActive(true);
                        break;
                }

                GameCanvas.Instance.BoldItem(equipedBlueprintSO.GetId());
            }
        }
    }
}
