using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScreen : MonoBehaviour
{
    bool buttonIconSet = false;
    bool songPlayed = false;

    [SerializeField] SelectableButton firstSelectedButton = null;
    [SerializeField] MenuScreen parentScreen = null;
    [SerializeField] SoundManager.Songs song = SoundManager.Songs.None;
    SoundManager soundManager;

    [SerializeField] GameObject[] additionalElements = null;

    public MenuScreen ParentScreen { get { return parentScreen; } }

    public static event Action<SelectableButton[]> OnFirstActivation;

    void Awake()
    {
        soundManager = SoundManager.Get();
    }

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

        if (soundManager.WwiseInitialized && song != SoundManager.Songs.None)
        {
            soundManager.PlaySong(song);
            songPlayed = true;
        }
    }

    void Start()
    {
        if (!songPlayed && song != SoundManager.Songs.None)
        {
            soundManager.PlaySong(song);
            songPlayed = true;
        }
    }

    void OnDisable()
    {
        foreach (GameObject element in additionalElements) element.SetActive(false);
    }
}