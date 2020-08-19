using System;
using UnityEngine;

public class SceneBitManager : MonoBehaviour
{
    public enum SceneBitTypes
    {
        Dialogue,
        Decision
    }

    [Serializable]
    public struct DialogueData
    {
        public string characterName;

        public SceneBitSO nextBit;

        [TextArea(3, 10)]
        public string[] sentences;
    }

    [Serializable]
    public struct DecisionData
    {
        [Serializable]
        public struct Option
        {
            [TextArea(1, 5)]
            public string text;
            public SceneBitSO nextBit;
        }

        public string title;

        public Option[] options;
    }

    public Transform canvas;
    public GameObject dialogueBoxPrefab;
    public GameObject decisionBoxPrefab;

    public SceneBitSO initialBit;

    void OnEnable()
    {
        Dialogue.OnNextBitExecution += Generate;
        Decision.OnNextBitExecution += Generate;
    }

    void Start()
    {
        Generate(initialBit);
    }

    void OnDisable()
    {
        Dialogue.OnNextBitExecution -= Generate;
        Decision.OnNextBitExecution -= Generate;
    }

    public void Generate(SceneBitSO bit)
    {
        switch (bit.type)
        {
            case SceneBitTypes.Dialogue:
                GameObject newDialogueBox = Instantiate(dialogueBoxPrefab, canvas);
                newDialogueBox.GetComponent<Dialogue>().Initialize(bit.dialogueData.characterName, bit.dialogueData.sentences, bit.dialogueData.nextBit);
                break;
            case SceneBitTypes.Decision:
                GameObject newDecisionBox = Instantiate(decisionBoxPrefab, canvas);
                newDecisionBox.GetComponent<Decision>().Initialize(bit.decisionData.title, bit.decisionData.options);
                break;
            default:
                break;
        }
    }
}