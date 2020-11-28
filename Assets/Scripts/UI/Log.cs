using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;

    void OnEnable()
    {
        scrollbar.value = 0f;
    }
}