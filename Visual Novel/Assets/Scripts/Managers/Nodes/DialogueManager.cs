using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueManager : MonoBehaviour
{
    public enum Character
    {
        Protagonist,
        Hoshi,
        Seijun,
        Shadow,
        Tadao
    }

    bool typing = false;

    int currentDialogueStripIndex = 0;

    IEnumerator typingCoroutine;

    public GameObject dialogue;
    public GameObject dialogueBox;
    public Image characterSR;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    NoodlesNodeMultipleDialogue node;

    public List<CharacterSO> characterPool;
    Queue<string> sentenceQueue = new Queue<string>();
    Dictionary<Character, CharacterSO> characters = new Dictionary<Character, CharacterSO>();

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnDialogue += Begin;
    }

    void Awake()
    {
        foreach (CharacterSO character in characterPool)
        {
            characters.Add(character.character, character);
        }
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
            Character key = node.dialogueStrips[currentDialogueStripIndex].character;
            if (characters.TryGetValue(key, out CharacterSO character))
            {
                nameText.text = character.characterName;
                characterSR.sprite = character.sprite;
            }

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