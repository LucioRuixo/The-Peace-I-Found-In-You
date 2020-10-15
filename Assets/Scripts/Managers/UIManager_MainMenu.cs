using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager_MainMenu : MonoBehaviour
{
    public enum SaveSelectionScreenMode
    {
        NewGame,
        LoadGame
    }

    [SerializeField] GameObject mainMenuFirstSelected = null, creditsScreenFirstSelected = null;
    [SerializeField] GameObject mainMenu = null, mainScreen = null, saveSelectionScreen = null, creditsScreen = null;
    [SerializeField] GameObject saveSlotButtonPrefab = null;
    [SerializeField] Transform saveSlotButtonContainer = null;
    [SerializeField] TextMeshProUGUI versionText = null;

    public static event Action<SaveSelectionScreenMode> OnSaveSelectionScreenEnabled;

    void Start()
    {
        versionText.text = "v" + Application.version;
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToSaveSelectionScreen(int mode)
    {
        mainScreen.SetActive(false);

        SaveSelectionScreenMode saveSelectionScreenMode = (SaveSelectionScreenMode)mode;
        for (int i = 0; i < SaveManager.Get().SaveSlotsAmount; i++)
        {
            SaveSlotButton newSaveSlotButton = Instantiate(saveSlotButtonPrefab, saveSlotButtonContainer).GetComponent<SaveSlotButton>();
            newSaveSlotButton.Initialize(i, saveSelectionScreenMode);
        }

        saveSelectionScreen.SetActive(true);

        OnSaveSelectionScreenEnabled?.Invoke((SaveSelectionScreenMode)mode);
    }

    public void GoToCreditsScreen()
    {
        mainMenu.SetActive(false);
        creditsScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsScreenFirstSelected);
    }

    public void Return()
    {
        creditsScreen.SetActive(false);
        mainMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void Quit()
    {
        Application.Quit();
    }
}