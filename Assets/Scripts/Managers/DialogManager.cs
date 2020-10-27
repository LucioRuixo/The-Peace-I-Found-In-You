using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviourSingleton<DialogManager>
{
    enum ButtonType
    {
        Positive,
        Negative,
        Close
    }

    [SerializeField] string defaultPositiveText = "";
    [SerializeField] string defaultNegativeText = "";
    [SerializeField] string defaultCloseText = "";

    [SerializeField] GameObject cover = null;
    [SerializeField] GameObject dialogPrefab = null;
    [SerializeField] Transform dialogContainer = null;

    public bool CoverActive { get { return cover.activeInHierarchy; } }

    void AddButtonToDialog(Dialog dialog, ButtonType buttonType, string buttonText, UnityAction onPressed)
    {
        string text = "";
        if (buttonText != null && buttonText != "") text = buttonText;
        else
        {
            switch (buttonType)
            {
                case ButtonType.Positive:
                    text = defaultPositiveText;
                    break;
                case ButtonType.Negative:
                    text = defaultNegativeText;
                    break;
                case ButtonType.Close:
                    text = defaultCloseText;
                    break;
                default:
                    break;
            }
        }

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        UnityAction action = () => CloseDialog(dialog.gameObject, currentSelected);
        if (onPressed != null) action += onPressed;

        dialog.AddButton(text, action);
    }

    void CloseDialog(GameObject menuObject, GameObject firstSelected)
    {
        cover.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelected);

        Destroy(menuObject);
    }

    public void GenerateDialog(string dialogText, UnityAction onClose)
    {
        cover.SetActive(true);

        Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Dialog newDialog = Instantiate(dialogPrefab, position, Quaternion.identity, dialogContainer).GetComponent<Dialog>();

        newDialog.text.text = dialogText;

        AddButtonToDialog(newDialog, ButtonType.Close, defaultCloseText, onClose);
    }

    public void GenerateDialog(string dialogText, string positiveText, UnityAction onPositive, string negativeText, UnityAction onNegative)
    {
        cover.SetActive(true);

        Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Dialog newDialog = Instantiate(dialogPrefab, position, Quaternion.identity, dialogContainer).GetComponent<Dialog>();

        newDialog.text.text = dialogText;

        AddButtonToDialog(newDialog, ButtonType.Positive, defaultPositiveText, onPositive);
        AddButtonToDialog(newDialog, ButtonType.Negative, defaultNegativeText, onNegative);
    }
}