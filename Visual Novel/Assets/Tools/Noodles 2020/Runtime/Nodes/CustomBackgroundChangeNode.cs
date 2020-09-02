using System;
using UnityEditor.UIElements;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomBackgroundChangeNode : NoodlesNode
	{
		public Sprite newBackground;

		public CustomBackgroundChangeNode()
		{
			title = "Background Change";
#if UNITY_EDITOR
			classNameString = typeof(CustomBackgroundChangeNodeVisual).AssemblyQualifiedName;
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
	}

#if UNITY_EDITOR
	public class CustomBackgroundChangeNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomBackgroundChangeNode backgroundChangeNode = nodeData as CustomBackgroundChangeNode;

			title = backgroundChangeNode.title;
		}
	}
#endif
}