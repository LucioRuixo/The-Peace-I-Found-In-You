using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomRouteChoiceNode : NoodlesNode
	{
		public RouteManager.Route route;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Route Choice";
#if UNITY_EDITOR
			classNameString = typeof(CustomRouteChoiceNodeVisual).AssemblyQualifiedName;
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
	public class CustomRouteChoiceNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomRouteChoiceNode animationNode = nodeData as CustomRouteChoiceNode;

			title = animationNode.title;

			var combo = new EnumField("Route", animationNode.route);
			combo.RegisterValueChangedCallback(evt => { animationNode.route = (RouteManager.Route)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}