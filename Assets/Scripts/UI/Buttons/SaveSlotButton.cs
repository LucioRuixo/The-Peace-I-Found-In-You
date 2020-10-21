using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    int slotIndex = -1;

    UIManager_MainMenu.SaveSelectionScreenMode saveSelectionScreenMode;

    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] GameObject confirmationMenuPrefab = null;
    [SerializeField] GameObject notificationMenuPrefab = null;
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

    void CloseMenu(GameObject menuObject, GameObject firstSelected)
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

        if (saveSelectionScreenMode == UIManager_MainMenu.SaveSelectionScreenMode.NewGame)
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
            ConfirmationMenu newConfirmationMenu = Instantiate(confirmationMenuPrefab, position, Quaternion.identity, confirmationMenuContainer).GetComponent<ConfirmationMenu>();

            newConfirmationMenu.text.text = "¿Crear nueva partida en este espacio de guardado?";
            newConfirmationMenu.positiveButton.onClick.AddListener(CreateNewGame);
            newConfirmationMenu.negativeButton.onClick.AddListener(() => CloseMenu(newConfirmationMenu.gameObject, currentSelected));
        }
        else
        {
            if (SaveManager.Get().FileExists(slotIndex))
            {
                GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

                Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
                ConfirmationMenu newConfirmationMenu = Instantiate(confirmationMenuPrefab, position, Quaternion.identity, confirmationMenuContainer).GetComponent<ConfirmationMenu>();

                newConfirmationMenu.text.text = "¿Cargar esta partida?";
                newConfirmationMenu.positiveButton.onClick.AddListener(LoadGame);
                newConfirmationMenu.negativeButton.onClick.AddListener(() => CloseMenu(newConfirmationMenu.gameObject, currentSelected));
            }
            else
            {
                GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

                Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
                NotificationMenu newNotificationMenu = Instantiate(notificationMenuPrefab, position, Quaternion.identity, confirmationMenuContainer).GetComponent<NotificationMenu>();

                newNotificationMenu.text.text = "Este espacio de guardado está vacío.";
                newNotificationMenu.closeButton.onClick.AddListener(() => CloseMenu(newNotificationMenu.gameObject, currentSelected));
            }
        }
    }
}