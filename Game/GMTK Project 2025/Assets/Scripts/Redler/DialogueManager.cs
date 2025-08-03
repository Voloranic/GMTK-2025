using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences = new Queue<string>();
    string currentSentence;

    public bool inDialogue;
    bool isWritingASentence;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;

    private void Start()
    {
        inDialogue = false;
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    [System.Obsolete]
    public void StartDialogue(DialogueSO dialogue)
    {
        Time.timeScale = 0.1f;
        //FindObjectOfType<Movement>().gameObject.GetComponentInChildren<Animator>().speed *= 10;

        dialoguePanel.SetActive(true);
        dialogueText.text = "";

        inDialogue = true;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    [System.Obsolete]
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();

        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        isWritingASentence = true;

        foreach (char character in sentence.ToCharArray())
        {
            if (!isWritingASentence) yield break;

            dialogueText.text += character;
            //FindObjectOfType<AudioSource>().PlayOneShot(typeSFX); //Didn't go well

            if (character == '.' || character == '!' || character == '?')
            {
                for (int i = 0; i < 25; i++)
                {
                    yield return null;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    yield return null;
                }
            }
        }

        isWritingASentence = false;
    }

    void EndSentence(string sentence)
    {
        dialogueText.text = sentence;
        isWritingASentence = false;
    }

    [System.Obsolete]
    private void EndDialogue()
    {
        Time.timeScale = 1f;
        //FindObjectOfType<Movement>().gameObject.GetComponentInChildren<Animator>().speed /= 10;

        inDialogue = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        FindObjectOfType<Movement>().EnableMovement();
    }

    [System.Obsolete]
    private void Update()
    {
        if (!inDialogue) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isWritingASentence)
            {
                DisplayNextSentence();
            }
            else
            {
                EndSentence(currentSentence);
            }
        }
    }
}
