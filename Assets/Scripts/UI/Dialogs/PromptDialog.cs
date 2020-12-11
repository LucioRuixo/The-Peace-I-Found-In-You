using UnityEngine;
using TMPro;

public class PromptDialog : Dialog
{
    [SerializeField] TMP_InputField inputField = null;

    public string Input { get { return inputField.text; } }
}