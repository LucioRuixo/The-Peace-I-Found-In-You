using System;
using UnityEngine;

public class DecisionCheckManager : MonoBehaviour
{
    bool lastDecisionGood;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnDecisionCheck += CheckLastDecision;
        DecisionButton.OnDecisionButtonPressed += SetLastDecision;
    }

    void OnDisable()
    {
        NoodleManager.OnDecisionCheck -= CheckLastDecision;
        DecisionButton.OnDecisionButtonPressed -= SetLastDecision;
    }

    void SetLastDecision(int portIndex)
    {
        lastDecisionGood = portIndex == 0 ? true : false;
    }

    void CheckLastDecision(CustomDecisionCheckNode node)
    {
        int index = lastDecisionGood ? 0 : 1;

        OnNodeExecutionCompleted?.Invoke(index);
    }
}