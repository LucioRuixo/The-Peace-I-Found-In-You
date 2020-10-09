using System;
using UnityEngine;
using nullbloq.Noodles;

public abstract class NodeController : MonoBehaviour
{
    public abstract Type NodeType { protected set;  get; }

    public static event Action<int> OnNodeExecutionCompleted;

    public abstract void Execute(NoodlesNode genericNode);

    public void CallNodeExecutionCompletion(int portIndex)
    {
        OnNodeExecutionCompleted?.Invoke(portIndex);
    }
}