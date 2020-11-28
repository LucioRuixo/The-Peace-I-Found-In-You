using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviourSingleton<DialogManager>
{
    public class Button
    {
        public string Text { get; }
        public ButtonType Type { get; }
        public UnityAction OnPressed { get; }

        public Button(string _text, ButtonType _type, UnityAction _onPressed)
        {
            Text = _text;
            Type = _type;
            OnPressed = _onPressed;
        }
    }

    public enum ButtonType
    {
        Positive,
        Negative,
        Close,
        Continue,
        Cancel
    }

    [SerializeField] GameObject cover = null;
    [SerializeField] GameObject defaultDialogPrefab = null;
    [SerializeField] GameObject promptDialogPrefab = null;
    [SerializeField] Transform dialogContainer = null;

    [Header("Default Button Texts")]
    [SerializeField] string defaultPositive = "";
    [SerializeField] string defaultNegative = "";
    [SerializeField] string defaultClose = "";
    [SerializeField] string defaultContinue = "";
    [SerializeField] string defaultCancel = "";

    public bool CoverActive { get { return cover.activeInHierarchy; } }

    Dialog GenerateDialog(GameObject prefab, string message, Button[] buttons)
    {
        cover.SetActive(true);

        Vector2 position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Dialog newDialog = Instantiate(prefab, position, Quaternion.identity, dialogContainer).GetComponent<Dialog>();

        newDialog.Message = message;

        foreach (Button button in buttons)
        {
            AddButtonToDialog(button, newDialog);
        }

        return newDialog;
    }

    void AddButtonToDialog(Button button, Dialog dialog)
    {
        string text = "";
        if (button.Text != null && button.Text != "") text = button.Text;
        else
        {
            switch (button.Type)
            {
                case ButtonType.Positive:
                    text = defaultPositive;
                    break;
                case ButtonType.Negative:
                    text = defaultNegative;
                    break;
                case ButtonType.Close:
                    text = defaultClose;
                    break;
                case ButtonType.Continue:
                    text = defaultContinue;
                    break;
                case ButtonType.Cancel:
                    text = defaultCancel;
                    break;
                default:
                    break;
            }
        }
        
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        UnityAction action = () => CloseDialog(dialog.gameObject, currentSelected);
        if (button.OnPressed != null) action += button.OnPressed;

        dialog.AddButton(text, action);
    }

    void CloseDialog(GameObject menuObject, GameObject firstSelected)
    {
        cover.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelected);

        Destroy(menuObject);
    }

    public Dialog DisplayMessageDialog(string message, string close, UnityAction onClose)
    {
        Button closeButton = new Button(close, ButtonType.Close, onClose);

        return GenerateDialog(defaultDialogPrefab, message, new Button[] { closeButton });
    }

    public Dialog DisplayConfirmDialog(string message, string positive, UnityAction onPositive, string negative, UnityAction onNegative)
    {
        Button positiveButton = new Button(positive, ButtonType.Positive, onPositive);
        Button negativeButton = new Button(negative, ButtonType.Negative, onNegative);

        return GenerateDialog(defaultDialogPrefab, message, new Button[] { positiveButton, negativeButton });
    }

    public PromptDialog DisplayPromptDialog(string message, string @continue, UnityAction<string> onContinue, string cancel, UnityAction onCancel)
    {
        PromptDialog newDialog = null;

        Button cancelButton = new Button(cancel, ButtonType.Cancel, onCancel);
        Button continueButton = new Button(@continue, ButtonType.Continue, () => { onContinue(newDialog.Input); });

        newDialog = GenerateDialog(promptDialogPrefab, message, new Button[] { continueButton, cancelButton }) as PromptDialog;
        return newDialog;
    }
}