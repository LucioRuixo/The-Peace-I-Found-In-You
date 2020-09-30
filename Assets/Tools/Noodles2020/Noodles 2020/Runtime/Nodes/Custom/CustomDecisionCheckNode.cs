using System;
using System.Collections.Generic;
using nullbloq.Noodles;

public class CustomDecisionCheckNode : NoodlesNode
{
	[Serializable]
	public class DialogueStrip
	{
		public CharacterManager.Character character;
		public CharacterManager.Status status;
		public string sentence;
	}

	public List<DialogueStrip> dialogueStrips = new List<DialogueStrip>();

	protected override void PreInit()
	{
		base.PreInit();

		title = "Last Decision Check";
#if UNITY_EDITOR
		classNameString = typeof(CustomDecisionCheckNodeVisual).AssemblyQualifiedName;
#endif
		width = 1500;
		height = 500;
	}

	protected override void PostInit()
	{
		base.PostInit();

		NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Input");
		inputPorts.Add(port);

		NoodlesPort portOut = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Good");
		outputPorts.Add(portOut);
		portOut = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Bad");
		outputPorts.Add(portOut);
	}

	public void RemoveDialogueStrip(int index)
	{
		dialogueStrips.RemoveAt(index);
	}
}

#if UNITY_EDITOR
public class CustomDecisionCheckNodeVisual : NoodlesNodeVisual
{
	public const int StripWidth = 300;
	public const int StripHeight = 60;

	public const int ScrollViewHeight = 500;

	protected override void CreateVisualsBody()
	{
		base.CreateVisualsBody();

		CustomDecisionCheckNode decisionCheckNode = nodeData as CustomDecisionCheckNode;

		title = decisionCheckNode.title;
	}
}
#endif