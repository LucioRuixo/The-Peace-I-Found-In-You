using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScreen : MonoBehaviour
{
    bool buttonIconSet = false;

    [SerializeField] SelectableButton firstSelectedButton = null;
    [SerializeField] MenuScreen parentScreen = null;

    [SerializeField] GameObject[] additionalElements = null;

    public MenuScreen ParentScreen { get { return parentScreen; } }

    public static event Action<SelectableButton[]> OnFirstActivation;

    void OnEnable()
    {
        if (!buttonIconSet)
        {
            SelectableButton[] buttons = transform.GetComponentsInChildren<SelectableButton>();
            OnFirstActivation?.Invoke(buttons);

            buttonIconSet = true;
        }

        foreach (GameObject element in additionalElements) element.SetActive(true);

        EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        firstSelectedButton.DisplayIcon();
    }

    void OnDisable()
    {
        foreach (GameObject element in additionalElements) element.SetActive(false);
    }
}