using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class NoodleManager : MonoBehaviour
{
    public Noodler noodler;

    public List<Noodle> noodles;

    public static event Action<CustomCharacterActionNode> OnCharacterAction;
    public static event Action<NoodlesNodeMultipleDialogue> OnDialogue;
    public static event Action<CustomDecisionNode> OnDecision;
    public static event Action<CustomMinigameNode> OnMinigame;
    public static event Action<CustomBackgroundChangeNode> OnBackgroundChange;
    public static event Action<CustomAnimationNode> OnAnimation;
    public static event Action<CustomIlustrationNode> OnIlutration;
    public static event Action OnGameFinished;

    void OnEnable()
    {
        ActionManager.OnNodeExecutionCompleted += CallNextNode;
        DialogueManager.OnNodeExecutionCompleted += CallNextNode;
        DecisionManager.OnNodeExecutionCompleted += CallNextNode;
        MinigameManager.OnNodeExecutionCompleted += CallNextNode;
        BackgroundManager.OnNodeExecutionCompleted += CallNextNode;
        AnimationManager.OnNodeExecutionCompleted += CallNextNode;
        IlustrationManager.OnNodeExecutionCompleted += CallNextNode;
    }

    void Start()
    {
        ExecuteNextNode(noodler.CurrentNode);
    }

    void OnDisable()
    {
        ActionManager.OnNodeExecutionCompleted -= CallNextNode;
        DialogueManager.OnNodeExecutionCompleted -= CallNextNode;
        DecisionManager.OnNodeExecutionCompleted -= CallNextNode;
        MinigameManager.OnNodeExecutionCompleted -= CallNextNode;
        BackgroundManager.OnNodeExecutionCompleted -= CallNextNode;
        AnimationManager.OnNodeExecutionCompleted -= CallNextNode;
        IlustrationManager.OnNodeExecutionCompleted -= CallNextNode;
    }

    void CallNextNode(int portIndex)
    {
        if (noodler.HasNextNode())
        {
            NoodlesNode node = noodler.Next(portIndex);

            if (node != null) ExecuteNextNode(node);
            else Debug.LogError("Node not found");
        }
        else
            Debug.Log("No noodles remaining");
    }

    void ExecuteNextNode(NoodlesNode node)
    {
        if (node is CustomCharacterActionNode characterNode)
            OnCharacterAction?.Invoke(characterNode);
        else if (node is NoodlesNodeMultipleDialogue dialogueNode)
            OnDialogue?.Invoke(dialogueNode);
        else if (node is CustomDecisionNode decisionNode)
            OnDecision?.Invoke(decisionNode);
        else if (node is CustomMinigameNode minigameNode)
            OnMinigame?.Invoke(minigameNode);
        else if (node is CustomBackgroundChangeNode backgroundChangeNode)
            OnBackgroundChange?.Invoke(backgroundChangeNode);
        else if (node is CustomAnimationNode animationNode)
            OnAnimation?.Invoke(animationNode);
        else if (node is CustomIlustrationNode ilustrationNode)
            OnIlutration?.Invoke(ilustrationNode);
        else if (node is NoodlesNodeBorder borderNode)
            if (!borderNode.isStartNode) OnGameFinished?.Invoke();
    }
}