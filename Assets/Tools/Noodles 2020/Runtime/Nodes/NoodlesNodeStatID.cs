namespace nullbloq.Noodles
{
	public class NoodlesNodeStatID : NoodlesNode
	{
		//public Zarlanga.StatID statID;

		protected override void PreInit()
		{
			base.PreInit();
			title = "NoodlesNodeStatID";
#if UNITY_EDITOR
			classNameString = typeof(NoodlesNodeStatIDVisual).AssemblyQualifiedName;
#endif
			width = 500;
			height = 500;
		}

	}
#if UNITY_EDITOR
	public class NoodlesNodeStatIDVisual : NoodlesNodeVisual
	{
		protected override void CreateVisualsBody()
		{
			NoodlesNodeStatID statIDNode = nodeData as NoodlesNodeStatID;

			title = statIDNode.title;

			//var textField = new TextField("");
			//textField.SetValueWithoutNotify(dialogueNode.dialogueText);
			//textField.RegisterValueChangedCallback(evt =>
			//{
			//	dialogueNode.dialogueText = evt.newValue;
			//	//title = evt.newValue;
			//});
			//textField.SetValueWithoutNotify(dialogueNode.dialogueText);
			//mainContainer.Add(textField);
		}
	}
#endif
}