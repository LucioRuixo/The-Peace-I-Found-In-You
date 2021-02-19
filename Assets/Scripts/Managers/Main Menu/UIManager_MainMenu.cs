using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] Transform menuCanvas = null;
    [SerializeField] RectTransform selectionIcon = null;
    [SerializeField] TextMeshProUGUI versionText = null;
    [SerializeField] MenuScreen initialScreen = null;
    MenuScreen currentScreen;
    FXManager fxManager;

    [Header("Save Slot Buttons: ")]
    [SerializeField] GameObject saveSlotButtonPrefab = null;
    [SerializeField] Transform saveSlotButtonContainer = null;
    List<GameObject> saveSlotButtons = new List<GameObject>();

    [Header("Background Change: ")]
    [SerializeField] float backgroundChangeTime = 15f;
    [SerializeField] float fadeDuration = 3f;
    [SerializeField] float alphaAccuracyRange = 0.1f;

    [SerializeField] GameObject backgroundContainer = null;

    [SerializeField] SpriteRenderer background1SR = null;
    [SerializeField] SpriteRenderer background2SR = null;

    [SerializeField] Sprite[] backgrounds = null;

    public static event Action<SaveSelectionScreenMode> OnSaveSelectionScreenEnabled;

    void Awake()
    {
        fxManager = FXManager.Get();

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);

        MenuScreen.OnFirstActivation += SetButtonIcon;
        ScreenEnablerButton.OnMenuScreenEnabled += UpdateCurrentScreen;
    }

    void Start()
    {
        versionText.text = "v" + Application.version;
        initialScreen.gameObject.SetActive(true);
        currentScreen = initialScreen;

        StartCoroutine(ChangeBackground());
    }

    void OnDestroy()
    {
        MenuScreen.OnFirstActivation -= SetButtonIcon;
        ScreenEnablerButton.OnMenuScreenEnabled -= UpdateCurrentScreen;
    }

    void SetButtonIcon(SelectableButton[] buttons)
    {
        foreach (SelectableButton button in buttons) button.SelectionIcon = selectionIcon;
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
            newSaveSlotButton.GetComponent<SelectableButton>().AdjustIconToTextLength = true;

            saveSlotButtons.Add(newSaveSlotButton);
        }
    }

    void UpdateCurrentScreen(MenuScreen newScreen)
    {
        currentScreen.gameObject.SetActive(false);

        currentScreen = newScreen;
    }

    void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToCurrentScreenParent()
    {
        currentScreen.gameObject.SetActive(false);

        currentScreen = currentScreen.ParentScreen;
        currentScreen.gameObject.SetActive(true);
    }

    public void LoadGameplay()
    {
        SceneLoadManager.Get().LoadGameplay();
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
        GenerateSaveSlotButtons((SaveSelectionScreenMode)mode);
    
        OnSaveSelectionScreenEnabled?.Invoke((SaveSelectionScreenMode)mode);
    }

    public void DisplayExitGameDialog()
    {
        DialogManager.Get().DisplayConfirmDialog(exitGameText, null, ExitGame, null, null);
    }

    public void DisplaySavesFolderPath()
    {
        DialogManager.Get().DisplayMessageDialog(SaveManager.Get().SavesFolderPath, null, null);
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