using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public enum Character
    {
        Protagonist,
        Hoshi,
        Seijun,
        Shadow,
        Tadao
    }

    public List<CharacterSO> characters;
    public Dictionary<Character, CharacterSO> characterDictionary = new Dictionary<Character, CharacterSO>();

    void Awake()
    {
        foreach (CharacterSO character in characters)
        {
            characterDictionary.Add(character.character, character);
        }
    }
}