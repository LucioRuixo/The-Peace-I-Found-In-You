using System;
using UnityEngine;
using UnityEngine.UI;

public class DecisionButton : MonoBehaviour
{
    int portIndex;

    public static event Action<int> OnDecisionButtonPressed;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(MakeDecision);
    }

    void MakeDecision()
    {
        OnDecisionButtonPressed?.Invoke(portIndex);
    }

    public void SetPortIndex(int index)
    {
        portIndex = index;
    }
}