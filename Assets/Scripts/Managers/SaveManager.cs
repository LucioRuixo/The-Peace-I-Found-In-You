using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class SaveManager : MonoBehaviourSingleton<SaveManager>
{
    public int SaveSlotsAmount { private set; get; } = 4;

    int loadedFileIndex = -1;

    [SerializeField] GameManager.GameData initialGameData = new GameManager.GameData();

    [SerializeField] DefaultAsset fileModificationGuide = null;

    public string SavesFolderPath { private set; get; }

    new void Awake()
    {
        base.Awake();

        SavesFolderPath = Application.persistentDataPath + "\\Saves";
        Directory.CreateDirectory(SavesFolderPath);

        string fileName = fileModificationGuide.name + ".txt";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string destFilePath = Path.Combine(SavesFolderPath, fileName);
        if (!File.Exists(destFilePath)) File.Copy(filePath, destFilePath);
    }

    string GetSaveFilePath(int fileIndex, bool saveAsJson)
    {
        string extension = saveAsJson ? ".json" : ".dat";
        return SavesFolderPath + "\\save" + fileIndex + extension;
    }

    void CreateFile(int fileIndex)
    {
        Debug.Log("creating file");

        FileStream file = File.Create(GetSaveFilePath(fileIndex, false));
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(file, initialGameData);

        file.Close();
    }

    public void SetLoadedFileIndex(int fileIndex, UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode)
    {
        loadedFileIndex = fileIndex;

        string filePath = GetSaveFilePath(fileIndex, false);
        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame || !File.Exists(filePath))
            CreateFile(fileIndex);
    }

    public void SaveFile(GameManager.GameData gameData, bool saveAsJson)
    {
        Debug.Log("saving file");

        string datPath = GetSaveFilePath(loadedFileIndex, false);
        string jsonPath = GetSaveFilePath(loadedFileIndex, true);
        string filePath = saveAsJson ? jsonPath : datPath;

        FileStream file = File.OpenWrite(filePath);
        if (saveAsJson)
        {
            if (File.Exists(datPath)) File.Delete(datPath);

            string data = JsonUtility.ToJson(gameData, true);

            StreamWriter streamWriter = new StreamWriter(file);
            streamWriter.Write(data);
            streamWriter.Close();
        }
        else
        {
            if (File.Exists(jsonPath)) File.Delete(jsonPath);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(file, gameData);
        }
        file.Close();
    }

    public GameManager.GameData LoadFile()
    {
        Debug.Log("loading file");

        string datPath = GetSaveFilePath(loadedFileIndex, false);
        string jsonPath = GetSaveFilePath(loadedFileIndex, true);
        if (File.Exists(datPath))
        {
            FileStream file = File.OpenRead(datPath);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameManager.GameData fileData = (GameManager.GameData)binaryFormatter.Deserialize(file);

            file.Close();

            return fileData;
        }
        else if (File.Exists(jsonPath))
        {
            FileStream file = File.OpenRead(jsonPath);

            StreamReader streamReader = new StreamReader(file);
            string data = streamReader.ReadToEnd();
            GameManager.GameData fileData = JsonUtility.FromJson<GameManager.GameData>(data);

            file.Close();

            return fileData;
        }
        else
        {
            Debug.LogError("The file you were trying to load could not be found");
            return new GameManager.GameData();
        }
    }

    public bool FileExists(int fileIndex)
    {
        string datPath = GetSaveFilePath(fileIndex, false);
        string jsonPath = GetSaveFilePath(fileIndex, true);

        return File.Exists(datPath) || File.Exists(jsonPath);
    }
}