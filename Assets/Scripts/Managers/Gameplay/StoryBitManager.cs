using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class StoryBitManager : MonoBehaviour
{
    Noodler noodler;

    [SerializeField] List<NodeController> nodeControllers = null;
    Dictionary<Type, NodeController> controllersByNodeType = new Dictionary<Type, NodeController>();

    public static event Action OnSceneFinished;

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
        NodeController.OnNodeExecutionCompleted += CheckForNextBit;
    }

    void OnDisable()
    {
        NodeController.OnNodeExecutionCompleted -= CheckForNextBit;
    }

    void CheckForNextBit(int portIndex)
    {
        if (noodler.HasNextNode())
        {
            NoodlesNode bit = noodler.Next(portIndex);

            if (bit != null) ExecuteBit(bit);
            else Debug.LogError("Node not found");
        }
        else Debug.Log("No noodles remaining");
    }

    public void ExecuteBit(NoodlesNode node)
    {
        Type key = node.GetType();
        if (controllersByNodeType.TryGetValue(key, out NodeController controller))
        {
            controller.Execute(node);

            return;
        }
        else if (node is NoodlesNodeBorder borderNode && !borderNode.isStartNode)
        {
            OnSceneFinished?.Invoke();

            return;
        }

        Debug.LogError("No known controller is associated with the given node");
    }
}