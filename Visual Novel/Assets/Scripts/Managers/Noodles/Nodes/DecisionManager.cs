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

    public static event Action<string> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NodeManager.OnDecision += Begin;
    }

    void Start()
    {
        buttons = new List<Button>();
    }

    void OnDisable()
    {
        NodeManager.OnDecision -= Begin;
    }

    void Begin(CustomDecisionNode node)
    {
        buttonContainer.SetActive(true);

        foreach (NoodlesPort port in node.outputPorts)
        {
            Button newButton = Instantiate(buttonPrefab, buttonContainer.transform).GetComponent<Button>();
            newButton.onClick.AddListener(End);
            newButton.onClick.AddListener(delegate { OnNodeExecutionCompleted(port.targetNodeGUID[0]); }); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Option"; // Hacer que el texto sea una variable que se tome desde el target node
        
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