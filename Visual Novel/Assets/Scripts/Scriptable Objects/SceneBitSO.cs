using UnityEngine;

[CreateAssetMenu(fileName = "New Scene Bit", menuName = "Scene Bit")]
public class SceneBitSO : ScriptableObject
{
    public SceneBitManager.SceneBitTypes type;

    public SceneBitManager.DialogueData dialogueData;
    public SceneBitManager.DecisionData decisionData;
}