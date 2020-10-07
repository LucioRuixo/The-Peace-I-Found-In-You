using System;
using UnityEngine;
using nullbloq.Noodles;

public class NoodleManager : MonoBehaviour
{
    int currentNoodleIndex = 0;

    RouteManager.Route currentRoute;

    [SerializeField] NodeManager nodeManager = null;
    [SerializeField] Noodler noodler = null;

    [SerializeField] Noodle[] hoshiRoute = null;
    [SerializeField] Noodle[] seijunRoute = null;

    public static event Action OnNoNoodlesRemaining;

    void OnEnable()
    {
        RouteManager.OnRouteChosen += SelectRoute;
        NodeManager.OnNoodleFinished += PlayNextScene;
    }

    void Start()
    {
        nodeManager.ExecuteNextNode(noodler.CurrentNode);
    }

    void OnDisable()
    {
        RouteManager.OnRouteChosen -= SelectRoute;
        NodeManager.OnNoodleFinished -= PlayNextScene;
    }

    void SelectRoute(RouteManager.Route selectedRoute)
    {
        currentRoute = selectedRoute;
    }

    void PlayNextScene()
    {
        if (currentRoute == RouteManager.Route.Hoshi)
            CheckForNextNoodle(hoshiRoute);
        else
            CheckForNextNoodle(seijunRoute);
    }

    void CheckForNextNoodle(Noodle[] noodles)
    {
        if (currentNoodleIndex < noodles.Length)
        {
            noodler.controller = noodles[currentNoodleIndex];
            noodler.ResetNoodle();
            nodeManager.ExecuteNextNode(noodler.CurrentNode);

            currentNoodleIndex++;
        }
        else OnNoNoodlesRemaining?.Invoke();
    }
}