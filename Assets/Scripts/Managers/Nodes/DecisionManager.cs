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

    List<Button> buttons;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnDecision += Begin;
    }

    void Awake()
    {
        buttons = new List<Button>();
    }

    void OnDisable()
    {
        NoodleManager.OnDecision -= Begin;
    }

    void Begin(CustomDecisionNode node)
    {
        buttonContainer.SetActive(true);

        int currentPortIndex = 0;
        foreach (NoodlesPort port in node.outputPorts)
        {
            int portIndex = currentPortIndex;
            Button newButton = Instantiate(buttonPrefab, buttonContainer.transform).GetComponent<Button>();
            newButton.onClick.AddListener(End);
            newButton.onClick.AddListener(delegate { OnNodeExecutionCompleted(portIndex); }); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = node.outputPorts[portIndex].text;
            buttons.Add(newButton);

            currentPortIndex++;
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