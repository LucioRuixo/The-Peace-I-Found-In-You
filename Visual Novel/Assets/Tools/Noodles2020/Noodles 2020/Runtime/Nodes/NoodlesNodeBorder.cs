using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class NoodlesNodeBorder : NoodlesNode
	{
		public bool isStartNode;

		protected override void PreInit()
		{
			base.PreInit();
#if UNITY_EDITOR
			classNameString = typeof(NoodlesNodeBorderVisual).AssemblyQualifiedName;
#endif
			width = 100;
			height = 100;
		}

		protected override void PostInit()
		{
			base.PostInit();
			NoodlesPort port = new NoodlesPort(Guid.NewGuid().ToString(), GUID, title);
			if (isStartNode)
				outputPorts.Add(port);
			else
				inputPorts.Add(port);
		}
	}
#if UNITY_EDITOR
	public class NoodlesNodeBorderVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsHeader()
		{
			//base.CreateVisualsHeader();
			NoodlesNodeBorder borderNode = nodeData as NoodlesNodeBorder;

			styleSheets.Add(Resources.Load<StyleSheet>("Node"));
			AddToClassList(borderNode.isStartNode ? "startNode" : "endNode");
		}

		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			title = nodeData.title;
			NoodlesNodeBorder borderNode = nodeData as NoodlesNodeBorder;
			if (borderNode == null)
			{
				Debug.LogError("ERROR");
				return;
			}
		}
	}
#endif
}