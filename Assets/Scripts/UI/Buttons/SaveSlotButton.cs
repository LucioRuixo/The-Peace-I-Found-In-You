using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    int slotIndex = -1;

    [SerializeField] string newGameText = null;
    [SerializeField] string loadGameText = null;
    [SerializeField] string emptySlotText = null;

    UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode;

    [SerializeField] TextMeshProUGUI text = null;
    DialogManager dialogManager;

    void Awake()
    {
        dialogManager = DialogManager.Get();
    }

    void Start()
    {
        text.text = "Archivo " + (slotIndex + 1);
    }

    void LoadGame(UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode)
    {
        SaveManager.Get().SetLoadedFileIndex(slotIndex, saveSelectionScreenMode);
        SceneManager.LoadScene("Gameplay");
    }

    public void Initialize(int _slotIndex, UIManager_MainMenu.SaveSelectionScreenMode _saveSelectionScreenMode)
    {
        slotIndex = _slotIndex;
        saveSelectionScreenMode = _saveSelectionScreenMode;
    }

    public void InstantiateConfirmationMenu()
    {
        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame)
            dialogManager.GenerateDialog(newGameText, null, () => LoadGame(saveSelectionScreenMode), null, null);
        else
        {
            if (SaveManager.Get().FileExists(slotIndex))
                dialogManager.GenerateDialog(loadGameText, null, () => LoadGame(saveSelectionScreenMode), null, null);
            else
                dialogManager.GenerateDialog(emptySlotText, null);
        }
    }
}