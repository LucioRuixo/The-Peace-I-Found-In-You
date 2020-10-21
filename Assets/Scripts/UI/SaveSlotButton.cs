using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class SaveSlotButton : MonoBehaviour
{
    int slotIndex = -1;

    UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode;

    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] GameObject confirmationMenuPrefab = null;
    GameObject cover;
    Transform confirmationMenuContainer;

    void Start()
    {
        text.text = "Archivo " + (slotIndex + 1);
    }

    void CreateNewGame()
    {
        SaveManager.Get().CreateFile(slotIndex);
        LoadGame();
    }

    void LoadGame()
    {
        SaveManager.Get().LoadFile(slotIndex);
        SceneManager.LoadScene("Gameplay");
    }

    void CloseConfirmationMenu(GameObject menuObject, GameObject firstSelected)
    {
        Debug.Log(firstSelected);

        cover.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);

        Destroy(menuObject);
    }

    public void Initialize(int _slotIndex, UIManager_MainMenu.SaveSelectionScreenMode _saveSelectionScreenMode, GameObject _cover, Transform _confirmationMenuContainer)
    {
        slotIndex = _slotIndex;
        saveSelectionScreenMode = _saveSelectionScreenMode;
        cover = _cover;
        confirmationMenuContainer = _confirmationMenuContainer;
    }

    public void InstantiateConfirmationMenu()
    {
        cover.SetActive(true);

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ConfirmationMenu newConfirmationMenu = Instantiate(confirmationMenuPrefab, position, Quaternion.identity, confirmationMenuContainer).GetComponent<ConfirmationMenu>();

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

        UnityAction negativeAction = () => CloseConfirmationMenu(newConfirmationMenu.gameObject, currentSelected);
        newConfirmationMenu.negativeButton.onClick.AddListener(negativeAction);
    }
}