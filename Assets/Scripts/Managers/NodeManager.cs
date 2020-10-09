using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class NodeManager : MonoBehaviour
{
    Noodler noodler;

    [SerializeField] List<NodeController> nodeControllers = null;
    Dictionary<Type, NodeController> controllersByNodeType = new Dictionary<Type, NodeController>();

    //public static event Action<NoodlesNodeMultipleDialogue> OnDialogue;
    //public static event Action<CustomCharacterActionNode> OnCharacterAction;
    //public static event Action<CustomDecisionNode> OnDecision;
    //public static event Action<CustomMinigameNode> OnMinigame;
    //public static event Action<CustomBackgroundChangeNode> OnBackgroundChange;
    //public static event Action<CustomAnimationNode> OnAnimation;
    //public static event Action<CustomDecisionCheckNode> OnDecisionCheck;
    //public static event Action<CustomMusicChangeNode> OnMusicChange;
    //public static event Action<CustomRouteChoiceNode> OnRouteChoice;
    public static event Action OnNoodleFinished;

    void Awake()
    {
        noodler = GetComponent<Noodler>();

        foreach (var controller in nodeControllers)
        {
            if (controller.NodeType != null)
                controllersByNodeType.Add(controller.NodeType, controller);
            else
                Debug.LogError("Controller node type is null in class " + controller.name);
        }
    }

    void OnEnable()
    {
        NodeController.OnNodeExecutionCompleted += CheckForNextNode;
        //foreach (Transform child in transform)
        //{
        //    NodeController nodeController = null;
        //    if (child.TryGetComponent(out nodeController))
        //        nodeController.OnNodeExecutionCompleted += CheckForNextNode;
        //    else
        //        Debug.LogError("Node controller not found in " + child.name);
        //}

        //DialogueController.OnNodeExecutionCompleted += CheckForNextNode;
        //ActionController.OnNodeExecutionCompleted += CheckForNextNode;
        //DecisionController.OnNodeExecutionCompleted += CheckForNextNode;
        //MinigameController.OnNodeExecutionCompleted += CheckForNextNode;
        //BackgroundController.OnNodeExecutionCompleted += CheckForNextNode;
        //AnimationController.OnNodeExecutionCompleted += CheckForNextNode;
        //DecisionCheckController.OnNodeExecutionCompleted += CheckForNextNode;
        //MusicController.OnNodeExecutionCompleted += CheckForNextNode;
        //RouteController.OnNodeExecutionCompleted += CheckForNextNode;
    }

    void OnDisable()
    {
        NodeController.OnNodeExecutionCompleted -= CheckForNextNode;
        //foreach (Transform child in transform)
        //{
        //    NodeController nodeController = null;
        //    if (child.TryGetComponent(out nodeController))
        //        nodeController.OnNodeExecutionCompleted -= CheckForNextNode;
        //    else
        //        Debug.LogError("Node controller not found in " + child.name);
        //}

        //DialogueController.OnNodeExecutionCompleted -= CheckForNextNode;
        //ActionController.OnNodeExecutionCompleted -= CheckForNextNode;
        //DecisionController.OnNodeExecutionCompleted -= CheckForNextNode;
        //MinigameController.OnNodeExecutionCompleted -= CheckForNextNode;
        //BackgroundController.OnNodeExecutionCompleted -= CheckForNextNode;
        //AnimationController.OnNodeExecutionCompleted -= CheckForNextNode;
        //DecisionCheckController.OnNodeExecutionCompleted -= CheckForNextNode;
        //MusicController.OnNodeExecutionCompleted -= CheckForNextNode;
        //RouteController.OnNodeExecutionCompleted -= CheckForNextNode;
    }

    void CheckForNextNode(int portIndex)
    {
        if (noodler.HasNextNode())
        {
            NoodlesNode node = noodler.Next(portIndex);

            if (node != null) ExecuteNode(node);
            else Debug.LogError("Node not found");
        }
        else Debug.Log("No noodles remaining");
    }

    public void ExecuteNode(NoodlesNode node)
    {
        NodeController controller = null;

        Type key = node.GetType();
        if (controllersByNodeType.TryGetValue(key, out controller))
        {
            controller.Execute(node);

            return;
        }
        else if (node is NoodlesNodeBorder borderNode && !borderNode.isStartNode)
        {
            OnNoodleFinished?.Invoke();

            return;
        }

        Debug.LogError("No known controller is associated with the given node");

        //if (node is CustomCharacterActionNode characterNode)
        //    OnCharacterAction?.Invoke(characterNode);
        //else if (node is NoodlesNodeMultipleDialogue dialogueNode)
        //    OnDialogue?.Invoke(dialogueNode);
        //else if (node is CustomDecisionNode decisionNode)
        //    OnDecision?.Invoke(decisionNode);
        //else if (node is CustomMinigameNode minigameNode)
        //    OnMinigame?.Invoke(minigameNode);
        //else if (node is CustomBackgroundChangeNode backgroundChangeNode)
        //    OnBackgroundChange?.Invoke(backgroundChangeNode);
        //else if (node is CustomAnimationNode animationNode)
        //    OnAnimation?.Invoke(animationNode);
        //else if (node is CustomDecisionCheckNode decisionCheckNode)
        //    OnDecisionCheck?.Invoke(decisionCheckNode);
        //else if (node is CustomMusicChangeNode musicChangeNode)
        //    OnMusicChange?.Invoke(musicChangeNode);
        //else if (node is CustomRouteChoiceNode routeDecisionNode)
        //    OnRouteChoice?.Invoke(routeDecisionNode);
        //else if (node is NoodlesNodeBorder borderNode)
        //    if (!borderNode.isStartNode) OnNoodleFinished?.Invoke();
    }
}