using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomIlustrationNode : NoodlesNode
	{
		public IlustrationManager.Ilustration ilustration;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Ilustration";
#if UNITY_EDITOR
			classNameString = typeof(CustomIlustrationNodeVisual).AssemblyQualifiedName;
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
	public class CustomIlustrationNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomIlustrationNode backgroundChangeNode = nodeData as CustomIlustrationNode;

			title = backgroundChangeNode.title;

			var combo = new EnumField("Ilustration", backgroundChangeNode.ilustration);
			combo.RegisterValueChangedCallback(evt => { backgroundChangeNode.ilustration = (IlustrationManager.Ilustration)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}