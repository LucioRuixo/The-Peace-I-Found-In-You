using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DecisionManager : MonoBehaviour
{
    public GameObject buttonContainer;
    public Button optionAButton;
    public Button optionBButton;
    TextMeshProUGUI optionAButtonText;
    TextMeshProUGUI optionBButtonText;

    void OnEnable()
    {
        SceneBitSO.OnDecision += Begin;
    }

    void Start()
    {
        optionAButtonText = optionAButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        optionBButtonText = optionBButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnDisable()
    {
        SceneBitSO.OnDecision -= Begin;
    }

    void Begin(SceneBitSO.DecisionData data)
    {
        buttonContainer.SetActive(true);

        optionAButtonText.text = data.optionA.text;
        optionAButton.onClick.AddListener(data.optionA.nextBit.Execute);
        optionAButton.onClick.AddListener(End);

        optionBButtonText.text = data.optionB.text;
        optionBButton.onClick.AddListener(data.optionB.nextBit.Execute);
        optionBButton.onClick.AddListener(End);
    }

    void End()
    {
        optionAButton.onClick.RemoveAllListeners();
        optionBButton.onClick.RemoveAllListeners();

        buttonContainer.SetActive(false);
    }
}