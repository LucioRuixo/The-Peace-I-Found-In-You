using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonContainer;

    public string Message { set { message.text = value; } get { return message.text; } }

    public void AddButton(string text, UnityAction action)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

        newButton.GetComponent<DialogButton>().Text.text = text;
        newButton.GetComponent<Button>().onClick.AddListener(action);
    }
}