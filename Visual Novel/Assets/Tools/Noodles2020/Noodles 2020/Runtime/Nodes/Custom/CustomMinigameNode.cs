using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomMinigameNode : NoodlesNode
	{
		public MinigameManager.Minigame minigame;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Minigame";
#if UNITY_EDITOR
			classNameString = typeof(CustomMinigameNodeVisual).AssemblyQualifiedName;
#endif
			width = 600;
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
			combo.RegisterValueChangedCallback(evt => { minigameNode.minigame = (MinigameManager.Minigame)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}