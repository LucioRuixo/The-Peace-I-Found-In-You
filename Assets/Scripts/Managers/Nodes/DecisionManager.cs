using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using nullbloq.Noodles;

[Serializable]
public class DecisionManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject buttonContainer;

    List<GameObject> buttons;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnDecision += Begin;
        DecisionButton.OnDecisionButtonPressed += End;
    }

    void Awake()
    {
        buttons = new List<GameObject>();
    }

    void OnDisable()
    {
        NoodleManager.OnDecision -= Begin;
        DecisionButton.OnDecisionButtonPressed -= End;
    }

    void Begin(CustomDecisionNode node)
    {
        buttonContainer.SetActive(true);

        int currentPortIndex = 0;
        foreach (NoodlesPort port in node.outputPorts)
        {
            int portIndex = currentPortIndex;

            GameObject newButton = Instantiate(buttonPrefab, buttonContainer.transform);

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = node.outputPorts[portIndex].text;
            newButton.GetComponent<DecisionButton>().SetPortIndex(portIndex);

            buttons.Add(newButton);

            currentPortIndex++;
        }
    }

    void End(int portIndex)
    {
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
        buttonContainer.SetActive(false);

        OnNodeExecutionCompleted?.Invoke(portIndex); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
    }
}