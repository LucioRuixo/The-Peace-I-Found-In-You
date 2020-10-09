using System;
using UnityEngine;
using nullbloq.Noodles;

public class NoodleManager : MonoBehaviour
{
    int currentNoodleIndex = 0;

    RouteController.Route currentRoute;

    [SerializeField] NodeManager nodeManager = null;
    [SerializeField] Noodler noodler = null;

    [SerializeField] Noodle[] hoshiRoute = null;
    [SerializeField] Noodle[] seijunRoute = null;

    public static event Action OnNoNoodlesRemaining;

    void OnEnable()
    {
        RouteController.OnRouteChosen += SelectRoute;
        NodeManager.OnNoodleFinished += PlayNextScene;
    }

    void Start()
    {
        nodeManager.ExecuteNode(noodler.CurrentNode);
    }

    void OnDisable()
    {
        RouteController.OnRouteChosen -= SelectRoute;
        NodeManager.OnNoodleFinished -= PlayNextScene;
    }

    void SelectRoute(RouteController.Route selectedRoute)
    {
        currentRoute = selectedRoute;
    }

    void PlayNextScene()
    {
        if (currentRoute == RouteController.Route.Hoshi)
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
            nodeManager.ExecuteNode(noodler.CurrentNode);

            currentNoodleIndex++;
        }
        else OnNoNoodlesRemaining?.Invoke();
    }
}