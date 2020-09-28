using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Bit", menuName = "Scene Bit")]
public class SceneBitSO : ScriptableObject
{
    public enum SceneBitTypes
    {
        Dialogue,
        Decision,
        LocationChange,
        Minigame
    }

    [Serializable]
    public struct DialogueData
    {
        public CharacterSO character;

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

        public List<Option> options;
    }

    [Serializable]
    public struct LocationChangeData
    {
        public Sprite background;

        public SceneBitSO nextBit;
    }

    [Serializable]
    public struct MinigameData
    {
        public SceneBitSO nextBit;
    }

    public SceneBitTypes type;

    public DialogueData dialogueData;
    public DecisionData decisionData;
    public LocationChangeData locationChangeData;
    public MinigameData minigameData;

    public void Execute()
    {
        //switch (type)
        //{
        //    case SceneBitTypes.Dialogue:
        //        if (OnDialogue != null) OnDialogue(dialogueData);
        //        break;
        //    case SceneBitTypes.Decision:
        //        if (OnDecision != null) OnDecision(decisionData);
        //        break;
        //    case SceneBitTypes.LocationChange:
        //        if (OnLocationChange != null) OnLocationChange(locationChangeData);
        //        break;
        //    case SceneBitTypes.Minigame:
        //        if (OnMinigame != null) OnMinigame(minigameData);
        //        break;
        //    default:
        //        break;
        //}
    }
}