using System;
using nullbloq.Noodles;

public class DecisionCheckController : NodeController
{
    public override Type NodeType { protected set; get; }

    public bool LastDecisionGood { private set; get; }

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomDecisionCheckNode);
    }

    void OnEnable()
    {
        SaveManager.OnGameDataLoaded += SetLoadedData;
        DecisionButton.OnDecisionButtonPressed += SetLastDecision;
        //NodeManager.OnDecisionCheck += CheckLastDecision;
    }

    void OnDisable()
    {
        SaveManager.OnGameDataLoaded -= SetLoadedData;
        DecisionButton.OnDecisionButtonPressed -= SetLastDecision;
        //NodeManager.OnDecisionCheck -= CheckLastDecision;
    }

    void SetLoadedData(SaveManager.SaveData loadedData)
    {
        LastDecisionGood = loadedData.lastDecisionGood;
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

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomDecisionCheckNode;

        CheckLastDecision(node);
    }
}