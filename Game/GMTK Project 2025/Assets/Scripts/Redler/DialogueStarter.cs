using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;

    [System.Obsolete]
    public void StartDialogue()
    {
        FindObjectOfType<Movement>().DisableMovement();

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
