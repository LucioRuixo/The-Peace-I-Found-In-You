using System;
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
        public int routeNoodleIndex;
        public RouteController.Route currentRoute;
        [HideInInspector] public bool lastDecisionGood;
        [HideInInspector] public string currentNodeGUID;
    }

    public int SaveSlotsAmount { private set; get; } = 3;

    string savesFolderPath;

    int loadedFileIndex = -1;

    [SerializeField] SaveData initialGameData;
    SaveData loadedData;

    Noodler noodler = null;
    NoodleManager noodleManager = null;
    DecisionCheckController decisionCheckController = null;

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
        noodler = GameObject.Find("Node Manager").GetComponent<Noodler>();
        noodleManager = GameObject.Find("Noodle Manager").GetComponent<NoodleManager>();
        decisionCheckController = GameObject.Find("Decision Check Controller").GetComponent<DecisionCheckController>();
    }

    void DereferenceDataComponents()
    {
        noodler = null;
        noodleManager = null;
        decisionCheckController = null;
    }

    void UpdateFileData()
    {
        try
        {
            loadedData.lastDecisionGood = decisionCheckController.LastDecisionGood;
            loadedData.routeNoodleIndex = noodleManager.RouteNoodleIndex;
            loadedData.currentRoute = noodleManager.CurrentRoute;
            loadedData.currentNodeGUID = noodler.CurrentNode.GUID;
        }
        catch(NullReferenceException e)
        {
            Debug.LogError(e.Message + "File data could not be fully updated because one of the needed components is null");
        }
    }

    public void CreateFile(int index)
    {
        FileStream file = File.Create(savesFolderPath + "\\save" + index + ".dat");
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(file, initialGameData);

        file.Close();
    }

    public void SaveFile()
    {
        string filePath = savesFolderPath + "\\save" + loadedFileIndex + ".dat";
        FileStream file = File.OpenWrite(filePath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        UpdateFileData();
        binaryFormatter.Serialize(file, loadedData);

        file.Close();
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
    }
}