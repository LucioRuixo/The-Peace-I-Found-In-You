using System;
using UnityEditor.UIElements;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public enum Minigames
	{
		Puzzle,
		CookingMinigame,
		FightingMinigame
	}

	public class CustomMinigameNode : NoodlesNode
	{
		public Minigames minigame;

		public CustomMinigameNode()
		{
			title = "Minigame";
#if UNITY_EDITOR
			classNameString = typeof(CustomMinigameNodeVisual).AssemblyQualifiedName;
#endif
			width = 500;
			height = 500;
		}

		protected override void PostInit()
		{
			base.PostInit();
			NoodlesPort inputPort = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Input");
			inputPorts.Add(inputPort);
			NoodlesPort outputPort = new NoodlesPort(Guid.NewGuid().ToString(), GUID, "Output");
			outputPorts.Add(outputPort);
		}
	}

#if UNITY_EDITOR
	public class CustomMinigameNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomMinigameNode minigameNode = nodeData as CustomMinigameNode;

			title = minigameNode.title;

			var combo = new EnumField("Minigame", minigameNode.minigame);
			mainContainer.Add(combo);
		}
	}
#endif
}