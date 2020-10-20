﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmationMenu : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button positiveButton;
    public Button negativeButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(negativeButton.gameObject);
    }
}