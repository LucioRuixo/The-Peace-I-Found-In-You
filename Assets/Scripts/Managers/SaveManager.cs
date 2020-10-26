using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviourSingleton<SaveManager>
{
    public int SaveSlotsAmount { private set; get; } = 4;

    string savesFolderPath;

    int loadedFileIndex = -1;

    [SerializeField] GameManager.GameData initialGameData = new GameManager.GameData();

    new void Awake()
    {
        base.Awake();

        savesFolderPath = Application.persistentDataPath + "\\Saves";
        Directory.CreateDirectory(savesFolderPath);
    }

    void CreateFile(int fileIndex)
    {
        Debug.Log("creating file");

        FileStream file = File.Create(savesFolderPath + "\\save" + fileIndex + ".dat");
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        binaryFormatter.Serialize(file, initialGameData);

        file.Close();
    }

    public void SetLoadedFileIndex(int fileIndex, UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode)
    {
        loadedFileIndex = fileIndex;

        string filePath = savesFolderPath + "\\save" + loadedFileIndex + ".dat";
        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame || !File.Exists(filePath))
            CreateFile(fileIndex);
    }

    public void SaveFile(GameManager.GameData gameData)
    {
        Debug.Log("saving file");

        string filePath = savesFolderPath + "\\save" + loadedFileIndex + ".dat";

        FileStream file = File.OpenWrite(filePath);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(file, gameData);

        file.Close();
    }

    public GameManager.GameData LoadFile()
    {
        Debug.Log("loading file");

        string filePath = savesFolderPath + "\\save" + loadedFileIndex + ".dat";
        if (File.Exists(filePath))
        {
            FileStream file = File.OpenRead(filePath);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameManager.GameData fileData = (GameManager.GameData)binaryFormatter.Deserialize(file);

            file.Close();

            return fileData;
        }
        else
        {
            Debug.LogError("The file you were trying to load could not be found");
            return new GameManager.GameData();
        }
    }

    public bool FileExists(int index)
    {
        string filePath = savesFolderPath + "\\save" + index + ".dat";

        return File.Exists(filePath);
    }
}