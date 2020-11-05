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
    float fontSize;

    Coroutine typingCoroutine;

    [SerializeField] GameObject dialogueBox = null;
    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI dialogueText = null;
    [SerializeField] Log log = null;
    Image dialogueBoxImage;
    NoodlesNodeMultipleDialogue node;

    public List<RectTransform> clickableRects = new List<RectTransform>();

    Queue<string> sentenceQueue = new Queue<string>();

    [Header("Sentence Typing: ")]
    [SerializeField] float letterDisplayWaitTime = 0.1f;
    [SerializeField] float pauseWaitTime = 0.5f;
    [SerializeField] float whisperFontSizeFactor = 0.5f;

    void Awake()
    {
        NodeType = typeof(NoodlesNodeMultipleDialogue);

        fontSize = dialogueText.fontSize;
        dialogueBoxImage = dialogueBox.GetComponent<Image>();
    }

    void OnEnable()
    {
        UIManager_Gameplay.OnLogStateChange += UpdateLogState;
    }

    void Update()
    {
        if (!dialogueBox.activeInHierarchy) return;

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
        dialogueBox.SetActive(true);

        ExecuteNextDialogueStrip();
    }

    void UpdateLogState(bool state)
    {
        logActive = state;
    }

    public void ExecuteNextDialogueStrip() // Cuando cambie sentence de DialogueStrip por una lista de oraciones volver a hacerla privada y hacer que Continue llame a DisplayNextSentence
    {
        if (currentDialogueStripIndex >= node.dialogueStrips.Count)
        {
            End();
            return;
        }

        CharacterManager.CharacterName name = node.dialogueStrips[currentDialogueStripIndex].character;
        CharacterSO character = CharacterManager.Get().GetCharacterSO(name);
        if (character)
        {
            dialogueBoxImage.sprite = character.dialogueBoxSprite;

            if (node.dialogueStrips[currentDialogueStripIndex].status == CharacterManager.Status.Known)
                characterName = character.nameText;
            else
                characterName = unknownCharacterName;

            nameText.text = characterName;
        }

        //DisplayNextSentence();
        //if (typing) StopCoroutine(typingCoroutine); // CREO que esta línea es innecesaria
        sentence = node.dialogueStrips[currentDialogueStripIndex].sentence;

        bool whispering = CheckForWhisper();
        typingCoroutine = StartCoroutine(TypeSentence(whispering));
        typing = true;

        currentDialogueStripIndex++;
    }

    bool CheckForWhisper()
    {
        return sentence.ToCharArray()[0] == '[';
    }

    void End()
    {
        currentDialogueStripIndex = 0;
        dialogueBox.SetActive(false);

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