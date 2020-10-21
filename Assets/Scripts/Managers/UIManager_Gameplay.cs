using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Gameplay : MonoBehaviour
{
    [SerializeField] GameObject dialogueCover = null;
    [SerializeField] GameObject log = null;
    [SerializeField] GameObject confirmationMenuPrefab = null;
    [SerializeField] Transform confirmationMenuContainer = null;
    [SerializeField] DialogueController dialogueController = null;

    void CloseConfirmationMenu(ConfirmationMenu confirmationMenu)
    {
        dialogueController.clickableRects.Remove(confirmationMenu.GetComponent<RectTransform>());
        Destroy(confirmationMenu.gameObject);
    }

    public void SetLogActive(bool state)
    {
        dialogueCover.SetActive(state);
        log.SetActive(state);
    }

    public void SaveGame()
    {
        SaveManager.Get().SaveFile();
    }

    public void Exit()
    {
        Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ConfirmationMenu newConfirmationMenu = Instantiate(confirmationMenuPrefab, position, Quaternion.identity, confirmationMenuContainer).GetComponent<ConfirmationMenu>();

        dialogueController.clickableRects.Add(newConfirmationMenu.GetComponent<RectTransform>());

        newConfirmationMenu.text.text = "¿Salir del juego?";
        newConfirmationMenu.positiveButton.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));
        newConfirmationMenu.negativeButton.onClick.AddListener(() => CloseConfirmationMenu(newConfirmationMenu));
    }
}