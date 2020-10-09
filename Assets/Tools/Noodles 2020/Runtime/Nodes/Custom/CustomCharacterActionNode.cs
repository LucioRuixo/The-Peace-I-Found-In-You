using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace nullbloq.Noodles
{
	public class CustomCharacterActionNode : NoodlesNode
	{
		public ActionController.Action action;
		public CharacterController.Character character;
		public int bodyIndex;
		public int armIndex;
		public int headIndex;

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
			CustomCharacterActionNode actionNode = nodeData as CustomCharacterActionNode;

			title = actionNode.title;

			var combo = new EnumField("Action", actionNode.action);
			combo.RegisterValueChangedCallback(evt => { actionNode.action = (ActionController.Action)evt.newValue; });
			mainContainer.Add(combo);

			combo = new EnumField("Character", actionNode.character);
			combo.RegisterValueChangedCallback(evt => { actionNode.character = (CharacterController.Character)evt.newValue; });
			mainContainer.Add(combo);

			var intField = new IntegerField("Body Index");
			intField.SetValueWithoutNotify(actionNode.bodyIndex);
			intField.RegisterValueChangedCallback(evt =>
			{
				actionNode.bodyIndex = evt.newValue;
			});
			mainContainer.Add(intField);

			intField = new IntegerField("Arm Index");
			intField.SetValueWithoutNotify(actionNode.armIndex);
			intField.RegisterValueChangedCallback(evt =>
			{
				actionNode.armIndex = evt.newValue;
			});
			mainContainer.Add(intField);

			intField = new IntegerField("Head Index");
			intField.SetValueWithoutNotify(actionNode.headIndex);
			intField.RegisterValueChangedCallback(evt =>
			{
				actionNode.headIndex = evt.newValue;
			});
			mainContainer.Add(intField);
		}
	}
#endif
}