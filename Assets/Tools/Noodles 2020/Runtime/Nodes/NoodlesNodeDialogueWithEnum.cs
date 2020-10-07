#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace nullbloq.Noodles
{
	public enum CharacterEnumTest
	{
		Tito,
		Pepe,
		Jose
	}

	public class NoodlesNodeDialogueWithEnum : NoodlesNodeDialogue
	{
		public CharacterEnumTest character;

		protected override void PreInit()
		{
			base.PreInit();
			title = "Dialogue Pa Lucio";
#if UNITY_EDITOR
			classNameString = typeof(NoodlesNodeDialogueWithEnumVisual).AssemblyQualifiedName;
#endif
			width = 500;
			height = 500;
		}
	}

#if UNITY_EDITOR
	public class NoodlesNodeDialogueWithEnumVisual : NoodlesNodeDialogueVisual
	{
		protected override void CreateVisualsBody()
		{
			NoodlesNodeDialogueWithEnum dialogueNodeDialogue = nodeData as NoodlesNodeDialogueWithEnum;
			var combo = new EnumField("Character", dialogueNodeDialogue.character);
			mainContainer.Add(combo);

			base.CreateVisualsBody();
		}
	}
#endif
}