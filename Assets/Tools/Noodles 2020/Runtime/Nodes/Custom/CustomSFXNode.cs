using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

namespace nullbloq.Noodles
{
	public class CustomSFXNode : NoodlesNode
	{
		public SoundManager.SFXs sfx;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Sound Effect";
#if UNITY_EDITOR
			classNameString = typeof(CustomSFXNodeVisual).AssemblyQualifiedName;
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
	public class CustomSFXNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomSFXNode musicChangeNode = nodeData as CustomSFXNode;

			title = musicChangeNode.title;

			var combo = new EnumField("Sound", musicChangeNode.sfx);
			combo.RegisterValueChangedCallback(evt => { musicChangeNode.sfx = (SoundManager.SFXs)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}