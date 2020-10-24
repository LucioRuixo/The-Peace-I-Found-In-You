using nullbloq.Noodles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct GameData
    {
        [HideInInspector] public bool lastDecisionGood;

        public int routeNoodleIndex;

        public RouteController.Route currentRoute;

        [HideInInspector] public string currentNodeGUID;

        [HideInInspector] public List<Character> charactersInScene;
    }

    GameData gameData;

    [SerializeField] NoodleManager noodleManager = null;
    [SerializeField] Noodler noodler = null;
    [SerializeField] DecisionCheckController decisionCheckController = null;
    [SerializeField] ActionController actionController = null;

    void Awake()
    {
        SetGameData();
    }

    void OnEnable()
    {
        UIManager_Gameplay.OnGameSave += SaveGameData;
        NoodleManager.OnNoNoodlesRemaining += GoToMainMenu;
    }

    void OnDisable()
    {
        UIManager_Gameplay.OnGameSave -= SaveGameData;
        NoodleManager.OnNoNoodlesRemaining -= GoToMainMenu;
    }

    void SetGameData()
    {
        gameData = SaveManager.Get().LoadFile();

        noodleManager.SetData(gameData);
        decisionCheckController.SetData(gameData);
        actionController.SetData(gameData);
    }

    void UpdateGameData()
    {
        gameData.lastDecisionGood = decisionCheckController.LastDecisionGood;
        gameData.routeNoodleIndex = noodleManager.RouteNoodleIndex;
        gameData.currentRoute = noodleManager.CurrentRoute;
        gameData.currentNodeGUID = noodler.CurrentNode.GUID;
        gameData.charactersInScene = actionController.GetCharactersInScene();
    }

    void SaveGameData()
    {
        UpdateGameData();
        SaveManager.Get().SaveFile(gameData);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}