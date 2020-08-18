using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Dialogue dialogue;

    Queue<string> dialogueSentences;

    void Start()
    {
        dialogueSentences = new Queue<string>();

        StartDialogue();
    }

    void StartDialogue()
    {
        nameText.text = dialogue.name;

        foreach (string sentence in dialogue.sentences)
        {
            dialogueSentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void EndDialogue()
    {
        dialogueText.text = "";

    }

    public void DisplayNextSentence()
    {
        if (dialogueSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueSentences.Dequeue();
    }
}