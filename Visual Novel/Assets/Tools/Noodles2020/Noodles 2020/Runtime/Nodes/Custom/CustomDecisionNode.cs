using System;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
    public class CustomDecisionNode : NoodlesNode
    {
		public CustomDecisionNode()
		{
			title = "Decision";
#if UNITY_EDITOR
			classNameString = typeof(CustomDecisionNodeVisual).AssemblyQualifiedName;
#endif
			width = 1000;
			height = 500;
		}

		protected override void PostInit()
		{
			base.PostInit();
			NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Input");
			inputPorts.Add(port);
		}

		public NoodlesPort AddOption()
		{
			NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Option " + (outputPorts.Count + 1), true);
			outputPorts.Add(port);
			return port;
		}

		public void RemoveOption(int index)
		{
			outputPorts.RemoveAt(index);
		}
	}

#if UNITY_EDITOR
	public class CustomDecisionNodeVisual : NoodlesNodeVisual
    {
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomDecisionNode decisionNode = nodeData as CustomDecisionNode;

			title = decisionNode.title;

			var button = new Button(AddChoicePort)
			{
				text = "Add Option"
			};
			titleButtonContainer.Add(button);
		}

		public void AddChoicePort()
		{
			CustomDecisionNode decisionNode = nodeData as CustomDecisionNode;
			NoodlesPort p = decisionNode.AddOption();

			NoodlesPortVisual newPort = CreateVisualPort(p, Direction.Output);
			outputContainer.Add(newPort);
			RefreshPorts();
			RefreshExpandedState();
		}

		protected override void OnRemoveVisualPort(NoodlesPort p, NoodlesPortVisual pv)
		{
			base.OnRemoveVisualPort(p, pv);
			outputContainer.Remove(pv);
		}
	}
#endif
}