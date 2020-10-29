using System;
using System.Collections;
using System.Collections.Generic;
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

    bool lerpingAlpha1 = false;
    bool lerpingAlpha2 = false;

    [SerializeField] string exitGameText = "";

    float cameraHeight;

    Vector2 cameraSize;

    [SerializeField] GameObject saveSlotButtonPrefab = null;
    [SerializeField] Transform saveSlotButtonContainer = null;
    [SerializeField] RectTransform selectionIcon = null;
    [SerializeField] TextMeshProUGUI versionText = null;
    FXManager fxManager;

    List<GameObject> saveSlotButtons = new List<GameObject>();

    [Header("Background Change: ")]
    [SerializeField] float backgroundChangeTime = 15f;
    [SerializeField] float fadeDuration = 3f;
    [SerializeField] float alphaAccuracyRange = 0.1f;

    [SerializeField] GameObject backgroundContainer = null;

    [SerializeField] SpriteRenderer background1SR = null;
    [SerializeField] SpriteRenderer background2SR = null;

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

    void Awake()
    {
        fxManager = FXManager.Get();

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
    }

    void Start()
    {
        versionText.text = "v" + Application.version;

        StartCoroutine(ChangeBackground());
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void SetBackground(Sprite newBackground, SpriteRenderer backgroundSR)
    {
        backgroundSR.sprite = newBackground;

        backgroundContainer.transform.localScale = Vector3.one;
        Vector2 spriteSize = backgroundSR.sprite.bounds.size;
        backgroundContainer.transform.localScale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;
    }

    void GenerateSaveSlotButtons(SaveSelectionScreenMode saveSelectionScreenMode)
    {
        for (int i = 0; i < SaveManager.Get().SaveSlotsAmount; i++)
        {
            GameObject newSaveSlotButton = Instantiate(saveSlotButtonPrefab, saveSlotButtonContainer);

            newSaveSlotButton.GetComponent<SaveSlotButton>().Initialize(i, saveSelectionScreenMode);
            newSaveSlotButton.GetComponent<SelectableButton>().SetSelectionIcon(selectionIcon);

            saveSlotButtons.Add(newSaveSlotButton);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneNameManager.Get().Gameplay);
    }

    public void DeleteSaveSlotButtons()
    {
        foreach (GameObject button in saveSlotButtons)
        {
            Destroy(button);
        }

        saveSlotButtons.Clear();
    }

    public void GoToSaveSelectionScreen(int mode)
    {
        mainScreen.SetActive(false);
        GenerateSaveSlotButtons((SaveSelectionScreenMode)mode);
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
        DialogManager.Get().GenerateDialog(exitGameText, null, ExitGame, null, null);
    }
    
    IEnumerator ChangeBackground()
    {
        int backgroundIndex = UnityEngine.Random.Range(0, backgrounds.Length - 1);
        SetBackground(backgrounds[backgroundIndex], background1SR);

        while (true)
        {
            yield return new WaitForSeconds(backgroundChangeTime);

            int newBackgroundIndex;
            do newBackgroundIndex = UnityEngine.Random.Range(0, backgrounds.Length - 1);
            while (newBackgroundIndex == backgroundIndex);
            backgroundIndex = newBackgroundIndex;

            lerpingAlpha1 = true;
            lerpingAlpha2 = true;

            if (background1SR.color.a > 1f - alphaAccuracyRange / 2f && background1SR.color.a < 1f + alphaAccuracyRange / 2f)
            {
                SetBackground(backgrounds[backgroundIndex], background2SR);

                fxManager.StartAlphaLerp0To1(background2SR, fadeDuration, () => lerpingAlpha2 = false);
                fxManager.StartAlphaLerp1To0(background1SR, fadeDuration, () => lerpingAlpha1 = false);
            }
            else
            {
                SetBackground(backgrounds[backgroundIndex], background1SR);

                fxManager.StartAlphaLerp0To1(background1SR, fadeDuration, () => lerpingAlpha1 = false);
                fxManager.StartAlphaLerp1To0(background2SR, fadeDuration, () => lerpingAlpha2 = false);
            }

            yield return new WaitUntil(() => lerpingAlpha1 == false && lerpingAlpha2 == false);
        }
    }
}