using System;
using nullbloq.Noodles;

public class RouteController : NodeController
{
    public enum Route
    {
        Hoshi,
        Seijun,
        None
    }

    public override Type NodeType { protected set; get; }

    public static event Action<Route> OnRouteChosen;

    void Awake()
    {
        NodeType = typeof(CustomRouteChoiceNode);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomRouteChoiceNode;

        OnRouteChosen?.Invoke(node.route);
        CallNodeExecutionCompletion(0);
    }
}