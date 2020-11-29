using System;
using nullbloq.Noodles;

public class SFXController : NodeController
{
    public override Type NodeType { protected set; get; }

    void Awake()
    {
        NodeType = typeof(CustomSFXNode);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomSFXNode;

        SoundManager.Get().PlaySFX(node.sfx);

        CallNodeExecutionCompletion(0);
    }
}