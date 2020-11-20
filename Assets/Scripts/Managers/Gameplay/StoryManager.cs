using System;
using UnityEngine;
using nullbloq.Noodles;

public class StoryManager : MonoBehaviour, ISaveComponent
{
    [SerializeField] StoryBitManager storyBitManager = null;
    [SerializeField] Noodler noodler = null;

    [SerializeField] Noodle initialScene = null;
    [SerializeField] Noodle[] hoshiRoute = null;
    [SerializeField] Noodle[] seijunRoute = null;

    public static event Action<string> OnNoodlerControllerSet;
    public static event Action OnNoScenesRemaining;

    public bool RouteExecutionStarted { private set; get; } = false;
    public int RouteSceneIndex { private set; get; } = 0;
    public RouteController.Route CurrentRoute { private set; get; } = RouteController.Route.None;

    void OnEnable()
    {
        RouteController.OnRouteChosen += SelectRoute;
        StoryBitManager.OnSceneFinished += PlayNextScene;
    }

    void Start()
    {
        if (RouteSceneIndex > 0) RouteExecutionStarted = true;

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
        if (RouteExecutionStarted) RouteSceneIndex++;
        else RouteExecutionStarted = true;

        if (RouteSceneIndex < scenes.Length)
        {
            noodler.controller = scenes[RouteSceneIndex];
            noodler.ResetNoodle();
            storyBitManager.ExecuteBit(noodler.CurrentNode);
        }
        else OnNoScenesRemaining?.Invoke();
    }

    public void SetLoadedData(SaveData loadedData)
    {
        RouteExecutionStarted = loadedData.routeExecutionStarted;
        RouteSceneIndex = loadedData.routeSceneIndex;
        CurrentRoute = loadedData.currentRoute;

        if (CurrentRoute != RouteController.Route.None)
            noodler.controller = CurrentRoute == RouteController.Route.Hoshi ? hoshiRoute[RouteSceneIndex] : seijunRoute[RouteSceneIndex];
        else
            noodler.controller = initialScene;

        OnNoodlerControllerSet?.Invoke(loadedData.currentNodeGUID);
    }
}