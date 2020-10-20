using System;
using System.Collections;
using UnityEngine;
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

    bool increasingAlpha = false;
    bool decreasingAlpha = false;

    [SerializeField] GameObject saveSlotButtonPrefab = null;
    [SerializeField] GameObject cover = null;
    [SerializeField] Transform saveSlotButtonContainer = null;
    [SerializeField] Transform confirmationMenuContainer = null;
    [SerializeField] RectTransform selectionIcon = null;
    [SerializeField] TextMeshProUGUI versionText = null;

    [Header("Background Change: ")]
    [SerializeField] float backgroundChangeTime = 15f;
    [SerializeField] float fadeDuration = 3f;

    [SerializeField] Image background1 = null;
    [SerializeField] Image background2 = null;

    [SerializeField] Sprite[] backgrounds = null;

    [Header("Screens: ")]
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject mainScreen = null;
    [SerializeField] GameObject saveSelectionScreen = null;
    [SerializeField] GameObject creditsScreen = null;
    [SerializeField] GameObject extrasScreen = null;

    [Header("Screens First Selected: ")]
    [SerializeField] GameObject mainMenuFirstSelected = null;
    [SerializeField] GameObject saveSelectionScreenFirstSelected = null;
    [SerializeField] GameObject creditsScreenFirstSelected = null;
    [SerializeField] GameObject extrasScreenFirstSelected = null;

    public static event Action<SaveSelectionScreenMode> OnSaveSelectionScreenEnabled;

    void Start()
    {
        versionText.text = "v" + Application.version;

        StartCoroutine(ChangeBackground());
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
            newSaveSlotButton.Initialize(i, saveSelectionScreenMode, cover, confirmationMenuContainer);

            newSaveSlotButton.GetComponent<SelectableButton>().SetSelectionIcon(selectionIcon);
        }

        saveSelectionScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(saveSelectionScreenFirstSelected);

        OnSaveSelectionScreenEnabled?.Invoke((SaveSelectionScreenMode)mode);
    }

    public void GoToCreditsScreen()
    {
        mainMenu.SetActive(false);
        creditsScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsScreenFirstSelected);
    }

    public void GoToExtrasScreen()
    {
        mainMenu.SetActive(false);
        extrasScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(extrasScreenFirstSelected);
    }

    public void Return()
    {
        saveSelectionScreen.SetActive(false);
        creditsScreen.SetActive(false);
        extrasScreen.SetActive(false);
        mainMenu.SetActive(true);
        mainScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    IEnumerator ChangeBackground()
    {
        int backgroundIndex = UnityEngine.Random.Range(0, backgrounds.Length - 1);
        background1.sprite = backgrounds[backgroundIndex];

        while (true)
        {
            yield return new WaitForSeconds(backgroundChangeTime);

            int newBackgroundIndex;
            do newBackgroundIndex = UnityEngine.Random.Range(0, backgrounds.Length - 1);
            while (newBackgroundIndex == backgroundIndex);
            backgroundIndex = newBackgroundIndex;

            increasingAlpha = true;
            decreasingAlpha = true;

            if (background1.color.a == 1f)
            {
                background2.sprite = backgrounds[backgroundIndex];

                StartCoroutine(IncreaseAlpha(background2));
                StartCoroutine(DecreaseAlpha(background1));
            }
            else
            {
                background1.sprite = backgrounds[backgroundIndex];

                StartCoroutine(IncreaseAlpha(background1));
                StartCoroutine(DecreaseAlpha(background2));
            }

            yield return new WaitUntil(() => increasingAlpha == false && decreasingAlpha == false);
        }
    }

    IEnumerator IncreaseAlpha(Image image)
    {
        float currentAlphaValue = 0f;

        while (currentAlphaValue < 1f)
        {
            float addedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue += addedValue;

            Color newColor = image.color;
            newColor.a = currentAlphaValue;
            image.color = newColor;

            yield return null;
        }

        increasingAlpha = false;
    }

    IEnumerator DecreaseAlpha(Image image)
    {
        float currentAlphaValue = 1f;

        while (currentAlphaValue > 0f)
        {
            float subtractedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue -= subtractedValue;

            Color newColor = image.color;
            newColor.a = currentAlphaValue;
            image.color = newColor;

            yield return null;
        }

        decreasingAlpha = false;
    }
}