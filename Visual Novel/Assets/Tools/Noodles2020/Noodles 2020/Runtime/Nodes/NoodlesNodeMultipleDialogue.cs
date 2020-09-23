using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;
using nullbloq.Noodles;

public class NoodlesNodeMultipleDialogue : NoodlesNode
{
	[Serializable]
	public class DialogueStrip
	{
		public CharacterManager.Character character;
		public string sentence;
		public string response;
	}

	public List<DialogueStrip> dialogueStrips = new List<DialogueStrip>();

	protected override void PreInit()
	{
		base.PreInit();

		title = "Multiple Dialogue";
#if UNITY_EDITOR
		classNameString = typeof(NoodlesNodeMultipleDialogueVisual).AssemblyQualifiedName;
#endif
		width = 1500;
		height = 500;
	}

	protected override void PostInit()
	{
		base.PostInit();
		NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Input");
		inputPorts.Add(port);
		NoodlesPort portOut = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Out");
		outputPorts.Add(portOut);
	}

	public DialogueStrip AddDialogueStrip()
	{
		DialogueStrip ds = new DialogueStrip();
		ds.character = 0;
		ds.sentence = "Dialogue";
		ds.response = "Response " + dialogueStrips.Count;

		dialogueStrips.Add(ds);
		return ds;
	}

	public void RemoveDialogueStrip(int index)
	{
		dialogueStrips.RemoveAt(index);
	}
}

#if UNITY_EDITOR
public class NoodlesNodeMultipleDialogueVisual : NoodlesNodeVisual
{
	public const int StripWidth = 300;
	public const int StripHeight = 60;

	public const int ScrollViewHeight = 500;

	private ScrollView sv;

	protected override void CreateVisualsBody()
	{
		base.CreateVisualsBody();

		NoodlesNodeMultipleDialogue dialogueNode = nodeData as NoodlesNodeMultipleDialogue;

		title = dialogueNode.title;

		var button = new Button(AddDialogueStrip)
		{
			text = "Add Dialogue"
		};
		titleButtonContainer.Add(button);

		sv = new ScrollView();
		sv.style.width = StripWidth + 20;
		sv.style.height = ScrollViewHeight;
		sv.showVertical = true;
		sv.horizontalScroller.SetEnabled(true);

		for (var i = 0; i < dialogueNode.dialogueStrips.Count; i++)
		{
			NoodlesNodeMultipleDialogue.DialogueStrip strip = dialogueNode.dialogueStrips[i];
			AddStrip(strip, i);
		}

		mainContainer.Add(sv);
	}

	public void AddDialogueStrip()
	{
		NoodlesNodeMultipleDialogue n = nodeData as NoodlesNodeMultipleDialogue;
		NoodlesNodeMultipleDialogue.DialogueStrip ds = n.AddDialogueStrip();
		AddStrip(ds, n.dialogueStrips.Count-1);

		RefreshExpandedState();
	}

	void AddStrip(NoodlesNodeMultipleDialogue.DialogueStrip ds, int index)
	{
		VisualElement container = new Box();
		container.style.flexDirection = FlexDirection.Column;

		var combo = new EnumField("Character", ds.character);
		combo.RegisterValueChangedCallback(evt => { ds.character = (CharacterManager.Character)evt.newValue; });
		container.Add(combo);

		var textField = new TextField("Dialogue");
		textField.style.width = StripWidth;
		textField.style.height = StripHeight;
		textField.multiline = true;
		
		textField.SetValueWithoutNotify(ds.sentence);
		textField.RegisterValueChangedCallback(evt =>
		{
			ds.sentence = evt.newValue;
		});
		container.Add(textField);

		var textFieldResponse = new TextField("Response");
		textFieldResponse.style.width = StripWidth;
		textFieldResponse.style.height = 20;
		textFieldResponse.SetValueWithoutNotify(ds.response);
		textFieldResponse.RegisterValueChangedCallback(evt =>
		{
			ds.response = evt.newValue;
		});
		container.Add(textFieldResponse);


		var deleteButton = new Button(() => RemoveDialogueStrip(container, index))
		{
			text = "Remove"
		};
		container.Add(deleteButton);

		sv.Add(container);
	}

	public void RemoveDialogueStrip(VisualElement container, int index)
	{
		sv.Remove(container);
		NoodlesNodeMultipleDialogue n = nodeData as NoodlesNodeMultipleDialogue;
		n.RemoveDialogueStrip(index);
	}
}
#endif