using System;
using UnityEngine;
using nullbloq.Noodles;

public class StoryManager : MonoBehaviour
{
    bool routeExecutionStarted = false;

    public int RouteSceneIndex { private set; get; } = 0;
    public RouteController.Route CurrentRoute { private set; get; } = RouteController.Route.None;

    [SerializeField] StoryBitManager storyBitManager = null;
    [SerializeField] Noodler noodler = null;

    [SerializeField] Noodle initialScene = null;
    [SerializeField] Noodle[] hoshiRoute = null;
    [SerializeField] Noodle[] seijunRoute = null;

    public static event Action<string> OnNoodlerControllerSet;
    public static event Action OnNoScenesRemaining;

    void OnEnable()
    {
        RouteController.OnRouteChosen += SelectRoute;
        StoryBitManager.OnSceneFinished += PlayNextScene;
    }

    void Start()
    {
        storyBitManager.ExecuteBit(noodler.CurrentNode);
    }

    void OnDisable()
    {
        RouteController.OnRouteChosen -= SelectRoute;
        StoryBitManager.OnSceneFinished -= PlayNextScene;
    }

    void SelectRoute(RouteController.Route selectedRoute)
    {
        CurrentRoute = selectedRoute;
    }

    void PlayNextScene()
    {
        if (CurrentRoute == RouteController.Route.Hoshi)
            CheckForNextScene(hoshiRoute);
        else
            CheckForNextScene(seijunRoute);
    }

    void CheckForNextScene(Noodle[] scenes)
    {
        if (RouteSceneIndex < scenes.Length)
        {
            if (routeExecutionStarted) RouteSceneIndex++;
            else routeExecutionStarted = true;

            noodler.controller = scenes[RouteSceneIndex];
            noodler.ResetNoodle();
            storyBitManager.ExecuteBit(noodler.CurrentNode);
        }
        else OnNoScenesRemaining?.Invoke();
    }

    public void SetData(GameManager.GameData loadedData)
    {
        RouteSceneIndex = loadedData.routeSceneIndex;
        CurrentRoute = loadedData.currentRoute;

        if (CurrentRoute != RouteController.Route.None)
            noodler.controller = CurrentRoute == RouteController.Route.Hoshi ? hoshiRoute[RouteSceneIndex] : seijunRoute[RouteSceneIndex];
        else
            noodler.controller = initialScene;

        OnNoodlerControllerSet?.Invoke(loadedData.currentNodeGUID);
    }
}