using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar = null;

    void OnEnable()
    {
        scrollbar.value = 0f;
    }
}