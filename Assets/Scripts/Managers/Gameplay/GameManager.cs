using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using nullbloq.Noodles;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct GameData
    {
        [HideInInspector] public bool lastDecisionGood;

        public int routeSceneIndex;

        public RouteController.Route currentRoute;

        [HideInInspector] public string currentNodeGUID;

        public BackgroundController.BackgroundData backgroundData;

        [HideInInspector] public List<Character> charactersInScene;
    }

    GameData gameData;

    [SerializeField] StoryManager storyManager = null;
    [SerializeField] Noodler noodler = null;
    [SerializeField] DecisionCheckController decisionCheckController = null;
    [SerializeField] BackgroundController backgroundController = null;
    [SerializeField] ActionController actionController = null;

    void Awake()
    {
        SetGameData();
    }

    void OnEnable()
    {
        UIManager_Gameplay.OnGameSave += SaveGameData;
        StoryManager.OnNoScenesRemaining += GoToMainMenu;
    }

    void OnDisable()
    {
        UIManager_Gameplay.OnGameSave -= SaveGameData;
        StoryManager.OnNoScenesRemaining -= GoToMainMenu;
    }

    void SetGameData()
    {
        gameData = SaveManager.Get().LoadFile();

        storyManager.SetData(gameData);
        decisionCheckController.SetData(gameData);
        backgroundController.SetData(gameData);
        actionController.SetData(gameData);
    }

    void UpdateGameData()
    {
        gameData.lastDecisionGood = decisionCheckController.LastDecisionGood;
        gameData.routeSceneIndex = storyManager.RouteSceneIndex;
        gameData.currentRoute = storyManager.CurrentRoute;
        gameData.currentNodeGUID = noodler.CurrentNode.GUID;
        gameData.backgroundData = backgroundController.CurrentBackgroundData;
        gameData.charactersInScene = actionController.CharactersInScene;
    }

    void SaveGameData(bool saveAsJson)
    {
        UpdateGameData();
        SaveManager.Get().SaveFile(gameData, saveAsJson);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneNameManager.Get().MainMenu);
    }
}