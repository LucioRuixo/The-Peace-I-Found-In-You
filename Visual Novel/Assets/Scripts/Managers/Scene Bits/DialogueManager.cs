using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject dialogueBox;
    public GameObject character;
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
        nameText.text = data.character._name;
        nextBit = data.nextBit;

        character.GetComponent<Image>().sprite = data.character.sprite;

        foreach (string sentence in data.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }

        dialogue.SetActive(true);
        DisplayNextSentence();
    }

    void End()
    {
        dialogue.SetActive(false);

        nextBit.Execute();
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