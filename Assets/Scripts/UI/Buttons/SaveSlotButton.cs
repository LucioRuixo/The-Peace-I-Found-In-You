using UnityEngine;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    int slotIndex = -1;

    [SerializeField] string preIndexButtonText = "";
    [SerializeField] string postIndexButtonText = "";
    [SerializeField] string newGameText = "";
    [SerializeField] string loadGameText = "";
    [SerializeField] string emptySlotText = "";

    UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode;

    [SerializeField] TextMeshProUGUI text = null;
    DialogManager dialogManager;

    void Awake()
    {
        dialogManager = DialogManager.Get();
    }

    void Start()
    {
        text.text = preIndexButtonText + (slotIndex + 1) + postIndexButtonText;
    }

    void LoadGame(UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode)
    {
        SaveManager.Get().SetLoadedFileIndex(slotIndex, saveSelectionScreenMode);

        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.LoadGame)
            SceneLoadManager.Get().LoadGameplay();
    }

    public void Initialize(int _slotIndex, UIManager_MainMenu.SaveSelectionScreenMode _saveSelectionScreenMode)
    {
        slotIndex = _slotIndex;
        saveSelectionScreenMode = _saveSelectionScreenMode;
    }

    public void GenerateGameLoadDialog()
    {
        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame)
            dialogManager.DisplayConfirmDialog(newGameText, null, () => LoadGame(saveSelectionScreenMode), null, null);
        else
        {
            if (SaveManager.Get().FileExists(slotIndex))
                dialogManager.DisplayConfirmDialog(loadGameText, null, () => LoadGame(saveSelectionScreenMode), null, null);
            else
                dialogManager.DisplayMessageDialog(emptySlotText, null, null);
        }
    }
}