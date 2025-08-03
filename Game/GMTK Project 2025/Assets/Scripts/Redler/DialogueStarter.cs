using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;

    private bool didAct;

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            StartDialogue();
        }
    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (didAct)
            {
                return;
            }

            didAct = true;

            StartDialogue();
        }
    }

    [System.Obsolete]
    public void StartDialogue()
    {
        FindObjectOfType<Movement>().DisableMovement();

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
