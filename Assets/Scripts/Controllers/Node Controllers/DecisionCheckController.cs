using System;
using nullbloq.Noodles;

public class DecisionCheckController : NodeController
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

    void SetLastDecision(int portIndex)
    {
        LastDecisionGood = portIndex == 0 ? true : false;
    }

    void CheckLastDecision(CustomDecisionCheckNode node)
    {
        int index = LastDecisionGood ? 0 : 1;

        CallNodeExecutionCompletion(index);
    }

    public void SetData(GameManager.GameData loadedData)
    {
        LastDecisionGood = loadedData.lastDecisionGood;
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomDecisionCheckNode;

        CheckLastDecision(node);
    }
}