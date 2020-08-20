using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    SceneBitSO nextBit;

    public Queue<string> sentenceQueue;

    void OnEnable()
    {
        SceneBitSO.OnDialogue += Begin;
    }

    void Start()
    {
        sentenceQueue = new Queue<string>();
    }

    void OnDisable()
    {
        SceneBitSO.OnDialogue -= Begin;
    }

    void Begin(SceneBitSO.DialogueData data)
    {
        dialogueBox.SetActive(true);

        nameText.text = data.characterName;
        nextBit = data.nextBit;

        foreach (string sentence in data.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }
    
        DisplayNextSentence();
    }

    void End()
    {
        nextBit.Execute();

        dialogueBox.SetActive(false);
    }

    public void DisplayNextSentence()
    {
        if (sentenceQueue.Count == 0)
        {
            End();
            return;
        }

        StartCoroutine(TypeSentence(sentenceQueue.Dequeue()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}