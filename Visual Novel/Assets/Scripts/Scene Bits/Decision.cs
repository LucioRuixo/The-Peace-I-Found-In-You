using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Decision : MonoBehaviour
{
    SceneBitManager.DecisionData data;

    public TextMeshProUGUI titleText;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    public static event Action<SceneBitSO> OnNextBitExecution;

    public void Initialize(string title, SceneBitManager.DecisionData.Option[] options)
    {
        data.title = title;
        data.options = options;

        titleText.text = data.title;

        foreach (SceneBitManager.DecisionData.Option option in data.options)
        {
            Button newButton = Instantiate(buttonPrefab, buttonContainer).GetComponent<Button>();
            TextMeshProUGUI newButtonText = newButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            newButton.onClick.AddListener(delegate { MakeDecision(option.nextBit); });
            newButtonText.text = option.text;
        }
    }

    public void MakeDecision(SceneBitSO nextBit)
    {
        if (OnNextBitExecution != null)
            OnNextBitExecution(nextBit);

        Destroy(gameObject);
    }
}