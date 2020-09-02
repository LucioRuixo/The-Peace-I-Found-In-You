using System;
using UnityEditor.UIElements;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public enum Characters
	{
		Hoshi,
		Seijun
	}

	public class CustomDialogueNode : NoodlesNode
	{
		public string dialogueText = "Hello World!";

		public Characters character;

		public CustomDialogueNode()
		{
			title = "Dialogue";
#if UNITY_EDITOR
			classNameString = typeof(CustomDialogueNodeVisual).AssemblyQualifiedName;
#endif
			width = 500;
			height = 500;
		}

		protected override void PostSet()
		{
			base.PostSet();
			NoodlesPort inputPort = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Input");
			inputPorts.Add(inputPort);
			NoodlesPort outputPort = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Output");
			outputPorts.Add(outputPort);
		}

		public NoodlesPort AddPhrase()
		{
			NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Phrase " + (outputPorts.Count + 1), true);
			outputPorts.Add(port);
			return port;
		}

		public void RemovePhrase(int index)
		{
			outputPorts.RemoveAt(index);
		}
	}

#if UNITY_EDITOR
	public class CustomDialogueNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomDialogueNode dialogueNode = nodeData as CustomDialogueNode;

			title = dialogueNode.title;

			var button = new Button(() => { AddPhraseField(); })
			{
				text = "Add Phrase"
			};
			titleButtonContainer.Add(button);

			var combo = new EnumField("Character", dialogueNode.character);
			mainContainer.Add(combo);
		}

		public void AddPhraseField()
		{
			CustomDialogueNode dialogueNode = nodeData as CustomDialogueNode;

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

		protected void OnRemovePhraseField(/*algo*/)
		{
			// algo
		}
	}
#endif
}