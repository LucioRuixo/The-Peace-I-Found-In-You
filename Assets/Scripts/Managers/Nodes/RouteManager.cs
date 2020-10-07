using System;
using UnityEngine;
using nullbloq.Noodles;

public class RouteManager : MonoBehaviour
{
    public enum Route
    {
        Hoshi,
        Seijun
    }

    public static event Action<Route> OnRouteChosen;
    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NodeManager.OnRouteChoice += ChooseRoute;
    }

    void OnDisable()
    {
        NodeManager.OnRouteChoice -= ChooseRoute;
    }

    void ChooseRoute(CustomRouteChoiceNode node)
    {
        OnRouteChosen?.Invoke(node.route);
        OnNodeExecutionCompleted?.Invoke(0);
    }
}