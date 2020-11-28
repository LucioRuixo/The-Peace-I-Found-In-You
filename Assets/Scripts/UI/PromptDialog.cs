using UnityEngine;
using TMPro;

public class PromptDialog : Dialog
{
    [SerializeField] TMP_InputField inputField;

    public string Input { get { return inputField.text; } }
}