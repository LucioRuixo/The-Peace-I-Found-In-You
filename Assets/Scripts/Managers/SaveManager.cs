using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviourSingleton<SaveManager>
{
    public int SaveSlotsAmount { private set; get; } = 4;

    int loadedFileIndex = -1;

    [SerializeField] SaveData initialGameData = new SaveData();

    [SerializeField] Object fileModificationGuide = null;

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

    string CreateSaveFilePath(int fileIndex, bool saveAsJson)
    {
        string extension = saveAsJson ? ".json" : ".dat";
        string fileName = "save" + fileIndex + extension;
        return Path.Combine(SavesFolderPath, fileName);
    }

    string GetSaveFilePath(int fileIndex)
    {
        string datPath = CreateSaveFilePath(fileIndex, false);
        string jsonPath = CreateSaveFilePath(fileIndex, true);

        if (File.Exists(datPath)) return datPath;
        else if (File.Exists(jsonPath)) return jsonPath;
        else return null;
    }

    void CreateFile(int fileIndex)
    {
        string jsonPath = CreateSaveFilePath(fileIndex, true);
        if (File.Exists(jsonPath)) File.Delete(jsonPath);

        FileStream file = File.Create(CreateSaveFilePath(fileIndex, false));
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(file, initialGameData);

        file.Close();
    }

    public void SetLoadedFileIndex(int fileIndex, UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode)
    {
        loadedFileIndex = fileIndex;

        string filePath = GetSaveFilePath(fileIndex);
        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame || !File.Exists(filePath))
            CreateFile(loadedFileIndex);
    }

    public void SaveFile(SaveData gameData, bool saveAsJson)
    {
        string datPath = CreateSaveFilePath(loadedFileIndex, false);
        string jsonPath = CreateSaveFilePath(loadedFileIndex, true);
        string filePath = saveAsJson ? jsonPath : datPath;

        if (saveAsJson)
        {
            if (File.Exists(datPath)) File.Delete(datPath);

            string data = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(jsonPath, data);
        }
        else
        {
            if (File.Exists(jsonPath)) File.Delete(jsonPath);

            FileStream file = File.OpenWrite(filePath);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(file, gameData);

            file.Close();
        }
    }

    public SaveData LoadFile()
    {
        string datPath = CreateSaveFilePath(loadedFileIndex, false);
        string jsonPath = CreateSaveFilePath(loadedFileIndex, true);
        if (File.Exists(datPath))
        {
            FileStream file = File.OpenRead(datPath);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            SaveData fileData = (SaveData)binaryFormatter.Deserialize(file);

            file.Close();

            return fileData;
        }
        else if (File.Exists(jsonPath))
        {
            FileStream file = File.OpenRead(jsonPath);

            string data = File.ReadAllText(jsonPath);
            SaveData fileData = JsonUtility.FromJson<SaveData>(data);

            file.Close();

            return fileData;
        }
        else
        {
            Debug.LogWarning("You won't be able to save the game because a save file wasn't chosen.");
            return initialGameData;
        }
    }

    public bool FileExists(int fileIndex)
    {
        string datPath = CreateSaveFilePath(fileIndex, false);
        string jsonPath = CreateSaveFilePath(fileIndex, true);

        return File.Exists(datPath) || File.Exists(jsonPath);
    }
}