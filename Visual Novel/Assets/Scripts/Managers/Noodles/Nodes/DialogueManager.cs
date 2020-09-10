using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueManager : MonoBehaviour
{
    int currentDialogueStripIndex = 0;

    public GameObject dialogue;
    public GameObject dialogueBox;
    public Image characterSR;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    NoodlesNodeMultipleDialogue node;

    public Queue<string> sentenceQueue;
    public List<CharacterSO> characterPool;
    Dictionary<string, CharacterSO> characters = new Dictionary<string, CharacterSO>();

    public static event Action<string> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NodeManager.OnDialogue += Begin;
    }

    void Awake()
    {
        sentenceQueue = new Queue<string>();

        foreach (CharacterSO character in characterPool)
        {
            characters.Add(character.characterName, character);
        }
    }

    void OnDisable()
    {
        NodeManager.OnDialogue -= Begin;
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
            string key = node.dialogueStrips[currentDialogueStripIndex].character.ToString();
            Debug.Log(key);
            if (characters.TryGetValue(key, out CharacterSO character))
            {
                nameText.text = character.characterName;
                characterSR.sprite = character.sprite;
            }

            //DisplayNextSentence();
            StartCoroutine(TypeSentence(node.dialogueStrips[currentDialogueStripIndex].sentence));
            currentDialogueStripIndex++;
        }
        else
            End();
    }

    void End()
    {
        currentDialogueStripIndex = 0;
        dialogue.SetActive(false);

        OnNodeExecutionCompleted?.Invoke(node.outputPorts[0].targetNodeGUID[0]); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
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
    }
}