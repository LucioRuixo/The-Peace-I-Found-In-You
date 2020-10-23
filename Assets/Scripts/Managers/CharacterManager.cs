using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public enum CharacterName
    {
        Protagonist,
        Hoshi,
        Seijun,
        Shadow1,
        Shadow2,
        Shadow3,
        Tadao,
        Sachi
    }

    public enum Status
    {
        Known,
        Unknown
    }

    public List<CharacterSO> characters;

    public CharacterSO GetCharacterSO(CharacterName name)
    {
        foreach (CharacterSO character in characters)
        {
            if (character.characterName == name) return character;
        }

        return null;
    }
}