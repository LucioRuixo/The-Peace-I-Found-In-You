using System;

[Serializable]
public class Character
{
    public int bodyIndex;
    public int armIndex;
    public int headIndex;

    public CharacterManager.CharacterName characterName;

    public Character(int _bodyIndex, int _armIndex, int _headIndex, CharacterManager.CharacterName _characterName)
    {
        bodyIndex = _bodyIndex;
        armIndex = _armIndex;
        headIndex = _headIndex;
        characterName = _characterName;
    }
}