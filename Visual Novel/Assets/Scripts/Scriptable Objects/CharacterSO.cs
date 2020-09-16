using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public DialogueManager.Character character;

    public string characterName;

    public Sprite sprite;
}