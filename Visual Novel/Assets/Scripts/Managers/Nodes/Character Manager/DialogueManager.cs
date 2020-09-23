using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class DialogueManager : MonoBehaviour
{
    bool typing = false;

    int currentDialogueStripIndex = 0;

    IEnumerator typingCoroutine;

    public GameObject dialogue;
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    CharacterManager characterManager;
    NoodlesNodeMultipleDialogue node;

    Queue<string> sentenceQueue = new Queue<string>();

    public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        characterManager = transform.parent.GetComponent<CharacterManager>();
    }

    void OnEnable()
    {
        NoodleManager.OnDialogue += Begin;
    }

    void Update()
    {
        if (!dialogue.activeInHierarchy) return;

        if (Input.GetButtonDown("Continue")) ExecuteNextDialogueStrip();
    }

    void OnDisable()
    {
        NoodleManager.OnDialogue -= Begin;
    }

    void Begin(NoodlesNodeMultipleDialogue _node)
    {
        node = _node;
        dialogue.SetActive(true);

        ExecuteNextDialogueStrip();
    }

    public void ExecuteNextDialogueStrip() // Cuando cambie sentence de DialogueStrip por una lista de oraciones volver a hacerla privada y hacer que Continue llame a DisplayNextSentence
    {
        if (node.dialogueStrips.Count > currentDialogueStripIndex)
        {
            CharacterManager.Character key = node.dialogueStrips[currentDialogueStripIndex].character;
            if (characterManager.characterDictionary.TryGetValue(key, out CharacterSO character))
                nameText.text = character.characterName;

            //DisplayNextSentence();
            if (typing) StopCoroutine(typingCoroutine);
            typingCoroutine = TypeSentence(node.dialogueStrips[currentDialogueStripIndex].sentence);
            StartCoroutine(typingCoroutine);
            typing = true;
            currentDialogueStripIndex++;
        }
        else
            End();
    }

    void End()
    {
        currentDialogueStripIndex = 0;
        dialogue.SetActive(false);

        OnNodeExecutionCompleted?.Invoke(0); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
    }

    //public void DisplayNextSentence()
    //{
    //    if (sentenceQueue.Count == 0)
    //    {
    //        End();
    //        return;
    //    }
    //
    //    StartCoroutine(TypeSentence(sentenceQueue.Dequeue()));
    //}

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        typing = false;
    }
}