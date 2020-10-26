using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class NodeManager : MonoBehaviour
{
    Noodler noodler;

    [SerializeField] List<NodeController> nodeControllers = null;
    Dictionary<Type, NodeController> controllersByNodeType = new Dictionary<Type, NodeController>();

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
    }

    void OnDisable()
    {
        NodeController.OnNodeExecutionCompleted -= CheckForNextNode;
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
    }
}