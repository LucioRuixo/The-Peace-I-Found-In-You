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
        public bool lastDecisionGood;
        public int routeNoodleIndex;
        public RouteController.Route currentRoute;
        public NoodlesNode currentNode;
    }

    string savesFolderPath;

    public int SaveSlotsAmount { private set; get; } = 3;

    int loadedFileIndex = -1;

    SaveData loadedData;

    [SerializeField] Noodler noodler;
    [SerializeField] NoodleManager noodleManager;
    [SerializeField] DecisionCheckController decisionCheckController;

    public static event Action<SaveData> OnGameDataLoaded;

    new void Awake()
    {
        base.Awake();

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
            OnGameDataLoaded?.Invoke(loadedData);
    }

    void CheckUnloadedScene(Scene scene)
    {
        if (scene.name == "Gameplay")
            loadedFileIndex = -1;
    }

    void UpdateFileData()
    {
        loadedData.lastDecisionGood = decisionCheckController.LastDecisionGood;
        loadedData.routeNoodleIndex = noodleManager.RouteNoodleIndex;
        loadedData.currentRoute = noodleManager.CurrentRoute;
        loadedData.currentNode = noodler.CurrentNode;
    }

    public void CreateFile(int index)
    {
        FileStream file = File.Create(savesFolderPath + "\\save" + index + ".dat");
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        loadedData.lastDecisionGood = false;
        loadedData.currentNode = null;

        binaryFormatter.Serialize(file, loadedData);

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
        else CreateFile(index);
    }
}