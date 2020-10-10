using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using nullbloq.Noodles;

[Serializable]
public class DecisionController : NodeController
{
    public override Type NodeType { protected set; get; }

    //[SerializeField] GameObject buttonPrefab = null;
    [SerializeField] DecisionButton option1Button = null, option2Button = null;
    [SerializeField] TextMeshProUGUI option1Text = null, option2Text = null;
    [SerializeField] GameObject decision = null;
    [SerializeField] Transform buttonContainer = null;

    //List<GameObject> buttons = new List<GameObject>();;

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomDecisionNode);
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
        option1Button.PortIndex = 0;
        option1Text.text = node.outputPorts[0].text;
        option1Button.gameObject.SetActive(true);

        option2Button.PortIndex = 1;
        option2Text.text = node.outputPorts[1].text;
        option2Button.gameObject.SetActive(true);

        decision.SetActive(true);

        //int currentPortIndex = 0;
        //foreach (NoodlesPort port in node.outputPorts)
        //{
        //    int portIndex = currentPortIndex;
        //
        //    GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
        //
        //    newButton.GetComponentInChildren<TextMeshProUGUI>().text = node.outputPorts[portIndex].text;
        //    newButton.GetComponent<DecisionButton>().SetPortIndex(portIndex);
        //
        //    buttons.Add(newButton);
        //
        //    currentPortIndex++;
        //}
    }

    void End(int portIndex)
    {
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);

        //foreach (GameObject button in buttons)
        //{
        //    Destroy(button);
        //}
        //buttons.Clear();
        decision.SetActive(false);

        CallNodeExecutionCompletion(portIndex); // Adaptar en NodeManager para que funcione al conectar el puerto con varios nodos en vez de uno solo
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomDecisionNode;

        Begin(node);
    }
}