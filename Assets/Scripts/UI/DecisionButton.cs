using System;
using UnityEngine;
using UnityEngine.UI;

public class DecisionButton : MonoBehaviour
{
    public int PortIndex { set; get; }

    public static event Action<int> OnDecisionButtonPressed;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(MakeDecision);
    }

    void MakeDecision()
    {
        OnDecisionButtonPressed?.Invoke(PortIndex);
    }
}