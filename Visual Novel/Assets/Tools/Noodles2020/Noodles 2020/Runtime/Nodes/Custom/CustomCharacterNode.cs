using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomCharacterActionNode : NoodlesNode
	{
		public ActionManager.Action action;
		public CharacterManager.Character character;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Character Action";
#if UNITY_EDITOR
			classNameString = typeof(CustomCharacterActionNodeVisual).AssemblyQualifiedName;
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
	public class CustomCharacterActionNodeVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			base.CreateVisualsBody();
			CustomCharacterActionNode characterNode = nodeData as CustomCharacterActionNode;

			title = characterNode.title;

			var combo = new EnumField("Action", characterNode.action);
			combo.RegisterValueChangedCallback(evt => { characterNode.action = (ActionManager.Action)evt.newValue; });
			mainContainer.Add(combo);

			combo = new EnumField("Character", characterNode.character);
			combo.RegisterValueChangedCallback(evt => { characterNode.character = (CharacterManager.Character)evt.newValue; });
			mainContainer.Add(combo);
		}
	}
#endif
}