using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] private BlueprintSO blueprintSO;

    private PlayerPickup playerPickupScript;

    private BlueprintsCollection playerBlueprintsCollectionScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerPickupScript == null)
            {
                playerPickupScript = other.transform.root.GetComponentInChildren<PlayerPickup>();

                playerBlueprintsCollectionScript = other.transform.root.GetComponentInChildren<BlueprintsCollection>();
            }

            playerPickupScript.AddBlueprintInPickupDistance(this);

            ShowPickupUI();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPickupScript.RemoveBlueprintInPickupDistance(this);

            HidePickupUI();
        }
    }

    private void ShowPickupUI()
    {
        //Call a function from the game ui script
        Debug.Log("Player in pickup distance.");
    }
    private void HidePickupUI()
    {
        //Call a function from the game ui script
        Debug.Log("Player out of pickup distance.");
    }

    public void Pickup()
    {
        playerBlueprintsCollectionScript.AddBlueprintToCollection(blueprintSO);
        Destroy(gameObject);
    }

}
