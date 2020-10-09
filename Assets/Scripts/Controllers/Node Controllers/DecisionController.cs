using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using nullbloq.Noodles;

[Serializable]
public class DecisionController : NodeController
{
    public override Type NodeType { protected set; get; }

    public GameObject buttonPrefab;
    public GameObject decision;
    public Transform buttonContainer;

    List<GameObject> buttons;

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomDecisionNode);

        buttons = new List<GameObject>();
    }

    void OnEnable()
    {
        //NodeManager.OnDecision += Begin;
        DecisionButton.OnDecisionButtonPressed += End;
    }

    void OnDisable()
    {
        //NodeManager.OnDecision -= Begin;
        DecisionButton.OnDecisionButtonPressed -= End;
    }

    void Begin(CustomDecisionNode node)
    {
        decision.SetActive(true);

        int currentPortIndex = 0;
        foreach (NoodlesPort port in node.outputPorts)
        {
            int portIndex = currentPortIndex;

            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

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
        decision.SetActive(false);

        CallNodeExecutionCompletion(portIndex); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomDecisionNode;

        Begin(node);
    }
}