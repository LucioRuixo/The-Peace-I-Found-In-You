using UnityEngine;
using TMPro;

public class DialogButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text = null;

    public TextMeshProUGUI Text { get { return text; } }
}