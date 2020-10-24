using System;
using UnityEngine;
using nullbloq.Noodles;

public class NoodleManager : MonoBehaviour
{
    public int RouteNoodleIndex { private set; get; } = 0;
    public RouteController.Route CurrentRoute { private set; get; } = RouteController.Route.None;

    [SerializeField] NodeManager nodeManager = null;
    [SerializeField] Noodler noodler = null;

    [SerializeField] Noodle initialNoodle = null;
    [SerializeField] Noodle[] hoshiRoute = null;
    [SerializeField] Noodle[] seijunRoute = null;

    public static event Action<string> OnNoodlerControllerSet;
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
        CurrentRoute = selectedRoute;
    }

    void PlayNextScene()
    {
        if (CurrentRoute == RouteController.Route.Hoshi)
            CheckForNextNoodle(hoshiRoute);
        else
            CheckForNextNoodle(seijunRoute);
    }

    void CheckForNextNoodle(Noodle[] noodles)
    {
        if (RouteNoodleIndex < noodles.Length)
        {
            noodler.controller = noodles[RouteNoodleIndex];
            noodler.ResetNoodle();
            nodeManager.ExecuteNode(noodler.CurrentNode);

            RouteNoodleIndex++;
        }
        else OnNoNoodlesRemaining?.Invoke();
    }

    public void SetData(GameManager.GameData loadedData)
    {
        RouteNoodleIndex = loadedData.routeNoodleIndex;
        CurrentRoute = loadedData.currentRoute;

        if (CurrentRoute != RouteController.Route.None)
            noodler.controller = CurrentRoute == RouteController.Route.Hoshi ? hoshiRoute[RouteNoodleIndex] : seijunRoute[RouteNoodleIndex];
        else
            noodler.controller = initialNoodle;

        OnNoodlerControllerSet?.Invoke(loadedData.currentNodeGUID);
    }
}