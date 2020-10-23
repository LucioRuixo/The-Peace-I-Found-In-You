using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using nullbloq.Noodles;

public class SaveManager : MonoBehaviourSingleton<SaveManager>
{
    [Serializable]
    public struct SaveData
    {
        [HideInInspector] public bool lastDecisionGood;

        public int routeNoodleIndex;

        public RouteController.Route currentRoute;

        [HideInInspector] public string currentNodeGUID;

        [HideInInspector] public List<Character> charactersInScene;
    }

    public int SaveSlotsAmount { private set; get; } = 4;

    string savesFolderPath;

    int loadedFileIndex = -1;

    [SerializeField] SaveData initialGameData;
    SaveData loadedData;

    Noodler noodler = null;
    NoodleManager noodleManager = null;
    DecisionCheckController decisionCheckController = null;
    ActionController actionController = null;

    public static event Action<SaveData> OnGameDataLoaded;

    new void Awake()
    {
        base.Awake();

        initialGameData.currentNodeGUID = null;

        savesFolderPath = Application.persistentDataPath + "\\Saves";
        Directory.CreateDirectory(savesFolderPath);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += CheckLoadedScene;
        SceneManager.sceneUnloaded += CheckUnloadedScene;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= CheckLoadedScene;
        SceneManager.sceneUnloaded -= CheckUnloadedScene;
    }

    void CheckLoadedScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay" && loadedFileIndex > -1)
        {
            ReferenceDataComponents();
            OnGameDataLoaded?.Invoke(loadedData);
        }
    }

    void CheckUnloadedScene(Scene scene)
    {
        if (scene.name == "Gameplay")
        {
            loadedFileIndex = -1;
            DereferenceDataComponents();
        }
    }

    void ReferenceDataComponents()
    {
        Debug.Log("referencing data components");

        noodler = GameObject.Find("Node Manager").GetComponent<Noodler>();
        if (!noodler) Debug.Log("noodler null");

        noodleManager = GameObject.Find("Noodle Manager").GetComponent<NoodleManager>();
        if (!noodleManager) Debug.Log("noodleManager null");

        decisionCheckController = GameObject.Find("Decision Check Controller").GetComponent<DecisionCheckController>();
        if (!decisionCheckController) Debug.Log("decisionCheckController null");

        actionController = GameObject.Find("Action Controller").GetComponent<ActionController>();
        if (!actionController) Debug.Log("actionController null");
    }

    void DereferenceDataComponents()
    {
        noodler = null;
        noodleManager = null;
        decisionCheckController = null;
        actionController = null;
    }

    void UpdateFileData()
    {
        try
        {
            loadedData.lastDecisionGood = decisionCheckController.LastDecisionGood;
            loadedData.routeNoodleIndex = noodleManager.RouteNoodleIndex;
            loadedData.currentRoute = noodleManager.CurrentRoute;
            loadedData.currentNodeGUID = noodler.CurrentNode.GUID;

            loadedData.charactersInScene = new List<Character>();
            foreach (KeyValuePair<Character, GameObject> character in actionController.GetCharactersInScene())
            {
                loadedData.charactersInScene.Add(character.Key);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogError(e.Message + ": File data could not be fully updated due to one of the required components being null");
        }
    }

    public void CreateFile(int index)
    {
        FileStream file = File.Create(savesFolderPath + "\\save" + index + ".dat");
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(file, initialGameData);

        file.Close();

        Debug.Log("file created");
    }

    public void SaveFile()
    {
        string filePath = savesFolderPath + "\\save" + loadedFileIndex + ".dat";
        FileStream file = File.OpenWrite(filePath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        UpdateFileData();
        binaryFormatter.Serialize(file, loadedData);

        file.Close();

        Debug.Log("file saved");
    }

    public void LoadFile(int index)
    {
        string filePath = savesFolderPath + "\\save" + index + ".dat";

        loadedFileIndex = index;

        if (File.Exists(filePath))
        {
            FileStream file = File.OpenRead(filePath);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            loadedData = (SaveData)binaryFormatter.Deserialize(file);

            file.Close();
        }
        else Debug.LogError("The file you were trying to load could not be found");

        Debug.Log("file loaded");
    }

    public bool FileExists(int index)
    {
        string filePath = savesFolderPath + "\\save" + index + ".dat";

        return File.Exists(filePath);
    }
}