using System;
using nullbloq.Noodles;

public class DecisionCheckController : NodeController
{
    public override Type NodeType { protected set; get; }

    bool lastDecisionGood;

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomDecisionCheckNode);
    }

    void OnEnable()
    {
        //NodeManager.OnDecisionCheck += CheckLastDecision;
        DecisionButton.OnDecisionButtonPressed += SetLastDecision;
    }

    void OnDisable()
    {
        //NodeManager.OnDecisionCheck -= CheckLastDecision;
        DecisionButton.OnDecisionButtonPressed -= SetLastDecision;
    }

    void SetLastDecision(int portIndex)
    {
        lastDecisionGood = portIndex == 0 ? true : false;
    }

    void CheckLastDecision(CustomDecisionCheckNode node)
    {
        int index = lastDecisionGood ? 0 : 1;

        CallNodeExecutionCompletion(index);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomDecisionCheckNode;

        CheckLastDecision(node);
    }
}