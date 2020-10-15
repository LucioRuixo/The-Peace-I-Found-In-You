using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    int slotIndex = -1;

    UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode;

    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] GameObject confirmationMenuPrefab = null;

    void Start()
    {
        text.text = "Archivo " + (slotIndex + 1);
    }

    void CreateNewGame()
    {
        SaveManager.Get().CreateFile(slotIndex);
        SceneManager.LoadScene("Gameplay");
    }

    void LoadGame()
    {
        SaveManager.Get().LoadFile(slotIndex);
        SceneManager.LoadScene("Gameplay");
    }

    public void Initialize(int _slotIndex, UIManager_MainMenu.SaveSelectionScreenMode _saveSelectionScreenMode)
    {
        slotIndex = _slotIndex;
        saveSelectionScreenMode = _saveSelectionScreenMode;
    }

    public void InstantiateConfirmationMenu()
    {
        Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ConfirmationMenu newConfirmationMenu = Instantiate(confirmationMenuPrefab, position, Quaternion.identity, transform).GetComponent<ConfirmationMenu>();

        string menuText = "";
        UnityAction positiveAction = null;
        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame)
        {
            menuText = "¿Crear nueva partida en este espacio de guardado?";
            positiveAction = CreateNewGame;
        }
        else
        {
            menuText = "¿Cargar esta partida?";
            positiveAction = LoadGame;
        }

        newConfirmationMenu.text.text = menuText;
        newConfirmationMenu.positiveButton.onClick.AddListener(positiveAction);
        newConfirmationMenu.negativeButton.onClick.AddListener(() => Destroy(newConfirmationMenu.gameObject));
    }
}