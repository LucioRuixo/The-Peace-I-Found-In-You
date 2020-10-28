using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using nullbloq.Noodles;

[Serializable]
public class DialogueController : NodeController
{
    public override Type NodeType { protected set; get; }

    bool typing = false;
    bool logActive = false;

    [SerializeField] char[] pauseCharacters = null;

    [SerializeField] string unknownCharacterName = "";
    string characterName;
    string sentence;


    int currentDialogueStripIndex = 0;

    public float letterDisplayWaitTime;
    public float pauseWaitTime;
    public float whisperFontSizeFactor = 0.5f;
    float fontSize;

    IEnumerator typingCoroutine;

    [SerializeField] GameObject dialogue = null;
    [SerializeField] Image dialogueBox = null;
    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI dialogueText = null;
    [SerializeField] Log log = null;
    NoodlesNodeMultipleDialogue node;

    public List<RectTransform> clickableRects = new List<RectTransform>();

    Queue<string> sentenceQueue = new Queue<string>();

    void Awake()
    {
        NodeType = typeof(NoodlesNodeMultipleDialogue);

        fontSize = dialogueText.fontSize;
    }

    void OnEnable()
    {
        UIManager_Gameplay.OnLogStateChange += UpdateLogState;
    }

    void Update()
    {
        if (!dialogue.activeInHierarchy) return;

        bool hoveringOverButton = false;
        foreach (RectTransform button in clickableRects)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(button, Input.mousePosition))
            {
                hoveringOverButton = true;
                break;
            }
        }

        if (!hoveringOverButton && !logActive && !DialogManager.Get().CoverActive && Input.GetButtonDown("Continue"))
        {
            if (typing)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = sentence;

                typing = false;
                log.AddSentence(characterName, sentence);
            }
            else ExecuteNextDialogueStrip();
        }
    }

    void OnDisable()
    {
        UIManager_Gameplay.OnLogStateChange -= UpdateLogState;
    }

    void Begin(NoodlesNodeMultipleDialogue _node)
    {
        node = _node;
        dialogue.SetActive(true);

        ExecuteNextDialogueStrip();
    }

    void UpdateLogState(bool state)
    {
        logActive = state;
    }

    public void ExecuteNextDialogueStrip() // Cuando cambie sentence de DialogueStrip por una lista de oraciones volver a hacerla privada y hacer que Continue llame a DisplayNextSentence
    {
        if (node.dialogueStrips.Count > currentDialogueStripIndex)
        {
            CharacterManager.CharacterName name = node.dialogueStrips[currentDialogueStripIndex].character;
            CharacterSO character = CharacterManager.Get().GetCharacterSO(name);
            if (character)
            {
                dialogueBox.sprite = character.dialogueBoxSprite;

                if (node.dialogueStrips[currentDialogueStripIndex].status == CharacterManager.Status.Known)
                    characterName = character.nameText;
                else
                    characterName = unknownCharacterName;

                nameText.text = characterName;
            }

            //DisplayNextSentence();
            if (typing) StopCoroutine(typingCoroutine);

            sentence = node.dialogueStrips[currentDialogueStripIndex].sentence;
            typingCoroutine = TypeSentence(CheckForWhisper());
            StartCoroutine(typingCoroutine);
            typing = true;

            currentDialogueStripIndex++;
        }
        else End();
    }

    bool CheckForWhisper()
    {
        return sentence.ToCharArray()[0] == '[';
    }

    void End()
    {
        currentDialogueStripIndex = 0;
        dialogue.SetActive(false);

        CallNodeExecutionCompletion(0); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
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

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as NoodlesNodeMultipleDialogue;

        Begin(node);
    }

    bool PauseFound(char character)
    {
        foreach (char pauseCharacter in pauseCharacters)
        {
            if (character == pauseCharacter) return true;
        }

        return false;
    }

    IEnumerator TypeSentence(bool whispering)
    {
        dialogueText.fontSize = fontSize;
        if (whispering) dialogueText.fontSize *= whisperFontSizeFactor;

        dialogueText.text = "";
        foreach (char character in sentence.ToCharArray())
        {
            dialogueText.text += character;

            if (PauseFound(character))
                yield return new WaitForSeconds(pauseWaitTime);

            yield return new WaitForSeconds(letterDisplayWaitTime);
        }

        typing = false;
        log.AddSentence(characterName, sentence);
    }
}