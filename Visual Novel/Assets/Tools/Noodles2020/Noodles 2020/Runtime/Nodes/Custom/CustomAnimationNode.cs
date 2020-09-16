using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomAnimationNode : NoodlesNode
	{
		public AnimationManager.Animation animation;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Animation";
#if UNITY_EDITOR
			classNameString = typeof(CustomAnimationNodeVisual).AssemblyQualifiedName;
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
	public class CustomAnimationNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomAnimationNode animationNode = nodeData as CustomAnimationNode;

			title = animationNode.title;

			var combo = new EnumField("Animation", animationNode.animation);
			combo.RegisterValueChangedCallback(evt => { animationNode.animation = (AnimationManager.Animation)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}