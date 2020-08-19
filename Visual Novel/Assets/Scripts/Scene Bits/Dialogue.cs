using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Dialogue : MonoBehaviour
{
    SceneBitManager.DialogueData data;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Queue<string> sentenceQueue;

    public static event Action<SceneBitSO> OnNextBitExecution;

    void Start()
    {
        sentenceQueue = new Queue<string>();

        StartDialogue();
    }

    void StartDialogue()
    {
        nameText.text = data.characterName;
    
        foreach (string sentence in data.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }
    
        DisplayNextSentence();
    }
    
    void EndDialogue()
    {
        if (OnNextBitExecution != null)
            OnNextBitExecution(data.nextBit);

        Destroy(gameObject);
    }

    public void Initialize(string characterName, string[] sentences, SceneBitSO nextBit)
    {
        data.characterName = characterName;
        data.sentences = sentences;
        data.nextBit = nextBit;
    }

    public void DisplayNextSentence()
    {
        if (sentenceQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
    
        dialogueText.text = sentenceQueue.Dequeue();
    }
}