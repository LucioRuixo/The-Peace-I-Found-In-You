using UnityEngine;

public class Character : MonoBehaviour
{
    public int BodyIndex { get; }
    public int ArmIndex { get; }
    public int HeadIndex { get; }

    public CharacterManager.CharacterName CharacterName { get; }

    public Character(int _bodyIndex, int _armIndex, int _headIndex, CharacterManager.CharacterName _characterName)
    {
        BodyIndex = _bodyIndex;
        ArmIndex = _armIndex;
        HeadIndex = _headIndex;
        CharacterName = _characterName;
    }
}