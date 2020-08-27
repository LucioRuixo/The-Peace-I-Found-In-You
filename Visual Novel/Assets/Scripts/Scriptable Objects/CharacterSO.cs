using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public string _name;

    public Sprite sprite;
}
