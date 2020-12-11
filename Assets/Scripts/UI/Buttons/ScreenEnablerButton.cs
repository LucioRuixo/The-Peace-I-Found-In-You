using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEnablerButton : MonoBehaviour
{
    [SerializeField] MenuScreen screen = null;
    Button button;

    static public event Action<MenuScreen> OnMenuScreenEnabled;

    void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(EnableScreen);
    }

    void EnableScreen()
    {
        OnMenuScreenEnabled?.Invoke(screen);

        screen.gameObject.SetActive(true);
    }
}