using System;
using UnityEngine;
using nullbloq.Noodles;

public class StoryManager : MonoBehaviour
{
    public int RouteNoodleIndex { private set; get; } = -1;
    public RouteController.Route CurrentRoute { private set; get; } = RouteController.Route.None;

    [SerializeField] StoryBitManager nodeManager = null;
    [SerializeField] Noodler noodler = null;

    [SerializeField] Noodle initialNoodle = null;
    [SerializeField] Noodle[] hoshiRoute = null;
    [SerializeField] Noodle[] seijunRoute = null;

    public static event Action<string> OnNoodlerControllerSet;
    public static event Action OnNoNoodlesRemaining;

    void OnEnable()
    {
        RouteController.OnRouteChosen += SelectRoute;
        StoryBitManager.OnNoodleFinished += PlayNextScene;
    }

    void Start()
    {
        nodeManager.ExecuteNode(noodler.CurrentNode);
    }

    void OnDisable()
    {
        RouteController.OnRouteChosen -= SelectRoute;
        StoryBitManager.OnNoodleFinished -= PlayNextScene;
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

    void CheckForNextNoodle(Noodle[] routeNoodles)
    {
        if (RouteNoodleIndex < routeNoodles.Length)
        {
            RouteNoodleIndex++;

            noodler.controller = routeNoodles[RouteNoodleIndex];
            noodler.ResetNoodle();
            nodeManager.ExecuteNode(noodler.CurrentNode);

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