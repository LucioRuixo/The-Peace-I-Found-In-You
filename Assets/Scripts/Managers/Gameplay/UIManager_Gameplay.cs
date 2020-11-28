using System;
using UnityEngine;
using UnityEngine.Events;

public class UIManager_Gameplay : MonoBehaviour
{
    [SerializeField] string exitGameText = null;
    [SerializeField] string saveGameText = null;
    [SerializeField] string saveAsJsonText = null;

    [SerializeField] GameObject log = null;
    DialogManager dialogManager;

    public static event Action<bool> OnGameSave;
    public static event Action<bool> OnLogStateChange;

    void Awake()
    {
        dialogManager = DialogManager.Get();
    }

    void AskForSaveExtension()
    {
        UnityAction saveAsJson = () => OnGameSave?.Invoke(true);
        UnityAction saveAsDat = () => OnGameSave?.Invoke(false);

        dialogManager.DisplayConfirmDialog(saveAsJsonText, null, saveAsJson, null, saveAsDat);
    }

    public void SetLogActive(bool state)
    {
        log.SetActive(state);

        OnLogStateChange?.Invoke(state);
    }

    public void SaveGame()
    {
        dialogManager.DisplayConfirmDialog(saveGameText, null, AskForSaveExtension, null, null);
    }

    public void ExitGame()
    {
        UnityAction positiveAction = () => SceneLoadManager.Get().LoadMainMenu();
        DialogManager.Get().DisplayConfirmDialog(exitGameText, null, positiveAction, null, null);
    }
}