using System;
using nullbloq.Noodles;

public class DecisionCheckController : NodeController, ISaveComponent
{
    public override Type NodeType { protected set; get; }

    public bool LastDecisionGood { private set; get; }

    void Awake()
    {
        NodeType = typeof(CustomDecisionCheckNode);
    }

    void OnEnable()
    {
        DecisionButton.OnDecisionButtonPressed += SetLastDecision;
    }

    void OnDisable()
    {
        DecisionButton.OnDecisionButtonPressed -= SetLastDecision;
    }

    void SetLastDecision(int decisionIndex)
    {
        LastDecisionGood = decisionIndex == 0 ? true : false;
    }

    void CheckLastDecision(CustomDecisionCheckNode node)
    {
        int index = LastDecisionGood ? 0 : 1;

        CallNodeExecutionCompletion(index);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomDecisionCheckNode;

        CheckLastDecision(node);
    }

    public void SetLoadedData(SaveData loadedData)
    {
        LastDecisionGood = loadedData.lastDecisionGood;
    }
}