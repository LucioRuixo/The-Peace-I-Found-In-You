using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

namespace nullbloq.Noodles
{
	public class CustomFilterNode : NoodlesNode
	{
		public FilterController.Filter filter;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Apply Filter";
#if UNITY_EDITOR
			classNameString = typeof(CustomFilterNodeVisual).AssemblyQualifiedName;
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
	public class CustomFilterNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomFilterNode filterNode = nodeData as CustomFilterNode;

			title = filterNode.title;

			var combo = new EnumField("Song Title", filterNode.filter);
			combo.RegisterValueChangedCallback(evt => { filterNode.filter = (FilterController.Filter)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}