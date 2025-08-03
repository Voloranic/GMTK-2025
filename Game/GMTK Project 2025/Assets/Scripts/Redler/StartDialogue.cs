using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;

            FindObjectOfType<Movement>().DisableMovement();

            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, gameObject);
        }
    }
}
