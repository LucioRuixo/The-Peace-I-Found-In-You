using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class NodeManager : MonoBehaviour
{
    const int firstNodeIndex = 2;
    int currentNodeIndex = firstNodeIndex;

    List<NoodlesNode> nodes = new List<NoodlesNode>();

    public static event Action<NoodlesNodeMultipleDialogue> OnDialogue;
    public static event Action<CustomDecisionNode> OnDecision;
    public static event Action<CustomMinigameNode> OnMinigame;
    public static event Action<CustomBackgroundChangeNode> OnBackgroundChange;

    void OnEnable()
    {
        NoodleManager.OnNoodleExecution += StartNoodleExecution;

        DialogueManager.OnNodeExecutionCompleted += ExecuteNextNode;
        DecisionManager.OnNodeExecutionCompleted += ExecuteNextNode;
        MinigameManager.OnNodeExecutionCompleted += ExecuteNextNode;
        BackgroundManager.OnNodeExecutionCompleted += ExecuteNextNode;
    }

    void OnDisable()
    {
        NoodleManager.OnNoodleExecution -= StartNoodleExecution;

        DialogueManager.OnNodeExecutionCompleted -= ExecuteNextNode;
        DecisionManager.OnNodeExecutionCompleted -= ExecuteNextNode;
        MinigameManager.OnNodeExecutionCompleted -= ExecuteNextNode;
        BackgroundManager.OnNodeExecutionCompleted -= ExecuteNextNode;
    }

    void StartNoodleExecution(List<NoodlesNode> _nodes, NoodlesNode startNode)
    {
        nodes = _nodes;

        ExecuteNextNode(startNode.GUID);
    }

    void ExecuteNextNode(string nodeGUID)
    {
        NoodlesNode node;

        if (GetNode(nodeGUID, out node))
        {
            if (nodes[currentNodeIndex] is NoodlesNodeMultipleDialogue dialogueNode)
                OnDialogue?.Invoke(dialogueNode);
            else if (nodes[currentNodeIndex] is CustomDecisionNode decisionNode)
                OnDecision?.Invoke(decisionNode);
            else if (nodes[currentNodeIndex] is CustomMinigameNode minigameNode)
                OnMinigame?.Invoke(minigameNode);
            else if (nodes[currentNodeIndex] is CustomBackgroundChangeNode backgroundChangeNode)
                OnBackgroundChange?.Invoke(backgroundChangeNode);

            //currentNodeIndex++;
        }
        else
        {
            Debug.LogError("Node not found");
            //currentNodeIndex = firstNodeIndex;
        }
    }

    bool GetNode(string _guid, out NoodlesNode node)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            node = nodes[i];
            if (node.GUID == _guid)
                return true;
        }

        node = null;
        return false;
    }
}