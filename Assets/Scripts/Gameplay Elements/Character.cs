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

    public enum AnimationType
    {
        None,
        HeadAndArms,
        HeadOnly,
        BodyOnly
    }

    float animationFadeTime = 0.5f;

    public int bodyIndex;
    public int armIndex;
    public int headIndex;
    public CharacterManager.CharacterName characterName;
    CharacterSO characterData;

    [Header("Sprite Properties: ")]
    [SerializeField] SpriteRenderer body = null;
    [SerializeField] SpriteRenderer arm = null;
    [SerializeField] SpriteRenderer head = null;

    [Header("Animation Properties: ")]
    [SerializeField] DragonBones.UnityArmatureComponent armature = null;
    [SerializeField] Collider rect;
    [SerializeField] AnimationType animationType;

    public bool Animated { private set; get; }

    bool SpriteIndexInsideRange(BodyPart bodyPart, int index)
    {
        if (!Animated)
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
        else return true;
    }

    public void Initialize(int _bodyIndex, int _armIndex, int _headIndex, CharacterManager.CharacterName _characterName)
    {
        bodyIndex = _bodyIndex;
        armIndex = _armIndex;
        headIndex = _headIndex;
        characterName = _characterName;

        Animated = armature;
        characterData = CharacterManager.Get().GetCharacterSO(characterName);

        ChangeBodyPart(BodyPart.Body, bodyIndex);
        ChangeBodyPart(BodyPart.Arm, armIndex);
        ChangeBodyPart(BodyPart.Head, headIndex);

        Vector2 position = transform.position;
        if (armature) position.y = ScreenManager.Get().MinScreenLimits.y + rect.bounds.extents.y - rect.transform.position.y;
        else position.y = ScreenManager.Get().MinScreenLimits.y + body.bounds.extents.y;
        transform.position = position;
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

        string animationName = "";
        switch (animationType)
        {
            case AnimationType.HeadAndArms:
                animationName = "brazo" + (armIndex + 1) + "-cabeza" + (headIndex + 1);
                break;
            case AnimationType.HeadOnly:
                animationName = "cabeza" + (headIndex + 1);
                break;
            case AnimationType.BodyOnly:
                animationName = "cuerpo" + (bodyIndex + 1);
                break;
            case AnimationType.None:
            default:
                break;
        }

        if (Animated)
        {
            //if (bodyPart != BodyPart.Body)
            //{
                //if (armature.animation.isPlaying)
                    armature.animation.FadeIn(animationName, animationFadeTime);
                //else armature.animation.Play(animationName);
            //}
        }
        else sr.sprite = sprites[index];
    }
}