using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public CharacterManager.CharacterName characterName;
    public string nameText;
    public Sprite dialogueBoxSprite;

    [Space]

    public Sprite[] bodySprites;
    public Sprite[] headSprites;
    public Sprite[] armSprites;

    [Space]

    public GameObject armatureObject;
}