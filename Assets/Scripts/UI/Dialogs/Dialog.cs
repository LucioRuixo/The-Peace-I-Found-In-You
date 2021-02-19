using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message = null;
    [SerializeField] GameObject buttonPrefab = null;
    [SerializeField] Transform buttonContainer = null;

    public string Message { set { message.text = value; } get { return message.text; } }

    public void AddButton(string text, UnityAction action, RectTransform selectionIcon)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

        newButton.GetComponent<DialogButton>().Text.text = text;
        newButton.GetComponent<Button>().onClick.AddListener(action);
        newButton.GetComponent<SelectableButton>().SelectionIcon = selectionIcon;
    }
}