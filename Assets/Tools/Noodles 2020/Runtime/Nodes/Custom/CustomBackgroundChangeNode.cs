using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

namespace nullbloq.Noodles
{
	public class CustomBackgroundChangeNode : NoodlesNode
	{
		public BackgroundManager.BackgroundType backgroundType;
		public BackgroundManager.Location location;
		public BackgroundManager.Ilustration ilustration;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Background Change";
#if UNITY_EDITOR
			classNameString = typeof(CustomBackgroundChangeNodeVisual).AssemblyQualifiedName;
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
	public class CustomBackgroundChangeNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomBackgroundChangeNode backgroundChangeNode = nodeData as CustomBackgroundChangeNode;

			title = backgroundChangeNode.title;
		
			var combo = new EnumField("Background Type", backgroundChangeNode.backgroundType);
			combo.RegisterValueChangedCallback(evt => { backgroundChangeNode.backgroundType = (BackgroundManager.BackgroundType)evt.newValue; });
			mainContainer.Add(combo);

			combo = new EnumField("Location", backgroundChangeNode.location);
			combo.RegisterValueChangedCallback(evt => { backgroundChangeNode.location = (BackgroundManager.Location)evt.newValue; });
			mainContainer.Add(combo);

			combo = new EnumField("Ilustration", backgroundChangeNode.ilustration);
			combo.RegisterValueChangedCallback(evt => { backgroundChangeNode.ilustration = (BackgroundManager.Ilustration)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}