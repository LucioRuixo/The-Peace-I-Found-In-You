using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class NoodleManager : MonoBehaviour
{
    int currentNoodleIndex = 0;

    public List<Noodle> noodles;

    public static event Action<List<NoodlesNode>, NoodlesNode> OnNoodleExecution;

    void Start()
    {
        CheckRemainingNoodles();
    }

    void CheckRemainingNoodles()
    {
        if (noodles.Count > 0)
        {
            List<NoodlesNode> nodeList = noodles[currentNoodleIndex].nodes;
            NoodlesNode starNode = noodles[currentNoodleIndex].GetStartNode();
            OnNoodleExecution?.Invoke(nodeList, starNode);

            currentNoodleIndex++;
        }
        else
            Debug.Log("No noodles remaining");
    }
}