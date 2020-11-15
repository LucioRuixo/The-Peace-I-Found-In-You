using System;
using UnityEngine;

[Serializable]
public class Character : MonoBehaviour
{
    public enum BodyPart
    {
        Body,
        Arm,
        Head
    }

    bool animated;

    float animationFadeTime = 0.5f;

    public int bodyIndex;
    public int armIndex;
    public int headIndex;
    public CharacterManager.CharacterName characterName;

    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer arm;
    [SerializeField] SpriteRenderer head;
    [SerializeField] DragonBones.UnityArmatureComponent armature;
    CharacterSO characterData;

    //public Character(int _bodyIndex, int _armIndex, int _headIndex, CharacterManager.CharacterName _characterName)
    //{
    //    bodyIndex = _bodyIndex;
    //    armIndex = _armIndex;
    //    headIndex = _headIndex;
    //    characterName = _characterName;
    //}

    bool SpriteIndexInsideRange(BodyPart bodyPart, int index)
    {
        if (!animated)
        {
            switch (bodyPart)
            {
                case BodyPart.Body:
                    return index >= 0 && index < characterData.bodySprites.Length;
                case BodyPart.Arm:
                    return index >= 0 && index < characterData.armSprites.Length;
                case BodyPart.Head:
                    return index >= 0 && index < characterData.headSprites.Length;
                default:
                    return false;
            }
        }
        else return true; //TODO: ampliar para que revise si existe una animación con el índice especificado
    }

    public void Initialize(int _bodyIndex, int _armIndex, int _headIndex, CharacterManager.CharacterName _characterName)
    {
        bodyIndex = _bodyIndex;
        armIndex = _armIndex;
        headIndex = _headIndex;
        characterName = _characterName;

        animated = armature;
        characterData = CharacterManager.Get().GetCharacterSO(characterName);

        ChangeBodyPart(BodyPart.Body, bodyIndex);
        ChangeBodyPart(BodyPart.Arm, armIndex);
        ChangeBodyPart(BodyPart.Head, headIndex);
    }

    public void ChangeBodyPart(BodyPart bodyPart, int index)
    {
        SpriteRenderer sr = null;
        Sprite[] sprites = null;

        switch (bodyPart)
        {
            case BodyPart.Body:
                bodyIndex = index;
                sr = body;
                sprites = characterData.bodySprites;
                break;
            case BodyPart.Arm:
                armIndex = index;
                sr = arm;
                sprites = characterData.armSprites;
                break;
            case BodyPart.Head:
                headIndex = index;
                sr = head;
                sprites = characterData.headSprites;
                break;
            default:
                break;
        }

        if (sprites.Length == 0) return;
        else
        {
            if (!SpriteIndexInsideRange(bodyPart, index))
            {
                Debug.LogError("Sprite index out of range");
                return;
            }
        }

        string animationName = "brazo" + (armIndex + 1) + "_cabeza" + (headIndex + 1);

        if (animated)
        {
            if (bodyPart != BodyPart.Body) //TODO: hacer que soporte cambios de animación de cuerpo
            {
                //if (armature.animation.isPlaying)
                    armature.animation.FadeIn(animationName, animationFadeTime);
                //else armature.animation.Play(animationName);
            }
        }
        else sr.sprite = sprites[index];
    }
}