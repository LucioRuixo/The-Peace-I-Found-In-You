using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Bit", menuName = "Scene Bit")]
public class SceneBitSO : ScriptableObject
{
    public enum SceneBitTypes
    {
        Dialogue,
        Decision,
        LocationChange
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

        public Option optionA;
        public Option optionB;
    }

    [Serializable]
    public struct LocationChangeData
    {
        public Sprite background;

        public SceneBitSO nextBit;
    }

    public SceneBitTypes type;

    public DialogueData dialogueData;
    public DecisionData decisionData;
    public LocationChangeData locationChangeData;

    public static event Action<DialogueData> OnDialogue;
    public static event Action<DecisionData> OnDecision;
    public static event Action<LocationChangeData> OnLocationChange;

    public void Execute()
    {
        switch (type)
        {
            case SceneBitTypes.Dialogue:
                if (OnDialogue != null) OnDialogue(dialogueData);
                break;
            case SceneBitTypes.Decision:
                if (OnDecision != null) OnDecision(decisionData);
                break;
            case SceneBitTypes.LocationChange:
                if (OnLocationChange != null) OnLocationChange(locationChangeData);
                break;
            default:
                break;
        }
    }
}