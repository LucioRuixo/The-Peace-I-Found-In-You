using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class NoodlesNodeDialogue : NoodlesNode
	{
		public string dialogueText = "Hello World!";

		protected override void PreInit()
		{
			base.PreInit();
			title = "Dialogue";
#if UNITY_EDITOR
			classNameString = typeof(NoodlesNodeDialogueVisual).AssemblyQualifiedName;
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
			NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Option " + outputPorts.Count, true);
			outputPorts.Add(port);
			return port;
		}

		public void RemoveOption(int index)
		{
			outputPorts.RemoveAt(index);
		}
	}
#if UNITY_EDITOR
	public class NoodlesNodeDialogueVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			NoodlesNodeDialogue dialogueNode = nodeData as NoodlesNodeDialogue;

			title = dialogueNode.title;

			var button = new Button(AddChoicePort)
			{
				text = "Add Choice"
			};
			titleButtonContainer.Add(button);

			var textField = new TextField("");
			textField.SetValueWithoutNotify(dialogueNode.dialogueText);
			textField.RegisterValueChangedCallback(evt =>
			{
				dialogueNode.dialogueText = evt.newValue;
				//title = evt.newValue;
			});
			textField.SetValueWithoutNotify(dialogueNode.dialogueText);
			mainContainer.Add(textField);
		}

		public void AddChoicePort()
		{
			NoodlesNodeDialogue dialogueNode = nodeData as NoodlesNodeDialogue;
			NoodlesPort p = dialogueNode.AddOption();

			NoodlesPortVisual newPort = CreateVisualPort(p, Direction.Output);
			outputContainer.Add(newPort);
			RefreshPorts();
			RefreshExpandedState();
		}

		protected override void OnRemoveVisualPort(NoodlesPort p, NoodlesPortVisual pv)
		{
			outputContainer.Remove(pv);
			base.OnRemoveVisualPort(p, pv);
		}
	}
#endif
}