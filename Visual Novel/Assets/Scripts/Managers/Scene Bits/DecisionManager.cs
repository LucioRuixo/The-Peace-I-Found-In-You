using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DecisionManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject buttonContainer;

    List<Button> buttons;

    void OnEnable()
    {
        SceneBitSO.OnDecision += Begin;
    }

    void Start()
    {
        buttons = new List<Button>();
    }

    void OnDisable()
    {
        SceneBitSO.OnDecision -= Begin;
    }

    void Begin(SceneBitSO.DecisionData data)
    {
        buttonContainer.SetActive(true);

        foreach (SceneBitSO.DecisionData.Option option in data.options)
        {
            Button newButton = Instantiate(buttonPrefab, buttonContainer.transform).GetComponent<Button>();
            newButton.onClick.AddListener(option.nextBit.Execute);
            newButton.onClick.AddListener(End);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = option.text;

            buttons.Add(newButton);
        }
    }

    void End()
    {
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();

        buttonContainer.SetActive(false);
    }
}