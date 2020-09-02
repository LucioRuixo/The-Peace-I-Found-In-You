using UnityEditor.UIElements;

namespace nullbloq.Noodles
{
	public enum CharacterEnumTest
	{
		Tito,
		Pepe,
		Jose
	}

	public class NoodlesNodeDialogueParaLucio : NoodlesNodeDialogue
	{
		public CharacterEnumTest character;

		public NoodlesNodeDialogueParaLucio()
		{
			title = "Dialogue Pa Lucio";
#if UNITY_EDITOR
			classNameString = typeof(NoodlesNodeDialogueParaLucioVisual).AssemblyQualifiedName;
#endif
			width = 500;
			height = 500;
		}
	}

	public class NoodlesNodeDialogueParaLucioVisual : NoodlesNodeDialogueVisual
	{
		protected override void CreateVisualsBody()
		{
			NoodlesNodeDialogueParaLucio dialogueNode = nodeData as NoodlesNodeDialogueParaLucio;
			var combo = new EnumField("Character", dialogueNode.character);
			mainContainer.Add(combo);

			base.CreateVisualsBody();
		}
	}
}