using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public CharacterManager.CharacterName characterName;

    public string nameText;

    public Sprite dialogueBoxSprite;

    public List<Sprite> bodySprites;
    public List<Sprite> headSprites;
    public List<Sprite> armSprites;
}