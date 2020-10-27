using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    [SerializeField] string exitText = null;

    [SerializeField] GameObject dialogueCover = null;
    [SerializeField] GameObject log = null;
    [SerializeField] GameObject confirmationMenuPrefab = null;
    [SerializeField] Transform confirmationMenuContainer = null;
    [SerializeField] DialogueController dialogueController = null;

    public static event Action OnGameSave;
    public static event Action<bool> OnLogStateChange;

    public void SetLogActive(bool state)
    {
        dialogueCover.SetActive(state);
        log.SetActive(state);

        OnLogStateChange?.Invoke(state);
    }

    public void SaveGame()
    {
        OnGameSave?.Invoke();
    }

    public void Exit()
    {
        DialogManager.Get().GenerateDialog(exitText, null, () => SceneManager.LoadScene("Main Menu"), null, null);
    }
}