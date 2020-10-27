using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    public void AddButton(string text, UnityAction action)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

        newButton.GetComponent<DialogButton>().Text.text = text;
        newButton.GetComponent<Button>().onClick.AddListener(action);
    }
}