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
    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomRouteChoiceNode);
    }

    void OnEnable()
    {
        //NodeManager.OnRouteChoice += ChooseRoute;
    }

    void OnDisable()
    {
        //NodeManager.OnRouteChoice -= ChooseRoute;
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomRouteChoiceNode;

        OnRouteChosen?.Invoke(node.route);
        CallNodeExecutionCompletion(0);
    }
}