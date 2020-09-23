using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public CharacterManager.Character character;

    public string characterName;

    public Sprite sprite;
}