using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomRouteChoiceNode : NoodlesNode
	{
		public RouteController.Route route;

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
			CustomRouteChoiceNode routeChoiceNode = nodeData as CustomRouteChoiceNode;

			title = routeChoiceNode.title;

			var combo = new EnumField("Route", routeChoiceNode.route);
			combo.RegisterValueChangedCallback(evt => { routeChoiceNode.route = (RouteController.Route)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}