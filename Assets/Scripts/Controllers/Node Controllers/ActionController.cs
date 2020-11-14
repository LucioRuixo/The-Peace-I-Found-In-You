using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nullbloq.Noodles;

public class ActionController : NodeController, ISaveComponent //TODO: separar esta clase en CharacterSpriteController y CharacterEnterExitController y hacer nodos para cada una
{
    public enum Action
    {
        EnterScene,
        ExitScene,
        PopIntoScene,
        PopOutOfScene,
        ChangeBody,
        ChangeHead,
        ChangeArm,
        FadeIntoScene,
        FadeOutOfScene
    }

    enum BodyPart
    {
        Body,
        Arm,
        Head
    }

    public override Type NodeType { protected set; get; }

    bool fadingOutOfScene = false;

    int activeCorroutines;

    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;
    float initialX;

    public Vector2 screenBounds;

    [SerializeField] GameObject characterPrefab = null;
    [SerializeField] Transform characterContainer = null;
    FXManager fxManager;

    List<KeyValuePair<Character, GameObject>> charactersInScene = new List<KeyValuePair<Character, GameObject>>();

    [Header("Enter/Exit Scene: ")]
    [SerializeField] float offScreenCharacterWidth = 1f;
    [SerializeField] float movementDuration = 1f;
    [SerializeField] float movementAccuracyRange = 1f;

    [Header("Fade Into/Off Scene: ")]
    [SerializeField] float fadeDuration = 1f;

    public List<Character> CharactersInScene { get
    {
        List<Character> characterKeys = new List<Character>();
        foreach (KeyValuePair<Character, GameObject> character in charactersInScene)
        {
            characterKeys.Add(character.Key);
        }

        return characterKeys;
    } }

    void Awake()
    {
        NodeType = typeof(CustomCharacterActionNode);

        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);
        SetScreenLimits();
        initialX = leftScreenLimit - offScreenCharacterWidth / 2f;

        fxManager = FXManager.Get();
    }

    void SetScreenLimits()
    {
        leftScreenLimit = screenBounds.x * -1f;
        rightScreenLimit = screenBounds.x;
        lowerScreenLimit = screenBounds.y;
        upperScreenLimit = screenBounds.y * -1f;
    }

    void Begin(CustomCharacterActionNode node)
    {
        switch (node.action)
        {
            case Action.EnterScene:
                EnterCharacter(node);
                break;
            case Action.ExitScene:
                ExitCharacter(node);
                break;
            case Action.PopIntoScene:
                EnterCharacter(node);
                break;
            case Action.PopOutOfScene:
                ExitCharacter(node);
                break;
            case Action.ChangeBody:
                ChangeBodyPart(BodyPart.Body, node);
                break;
            case Action.ChangeArm:
                ChangeBodyPart(BodyPart.Arm, node);
                break;
            case Action.ChangeHead:
                ChangeBodyPart(BodyPart.Head, node);
                break;
            case Action.FadeIntoScene:
                EnterCharacter(node);
                break;
            case Action.FadeOutOfScene:
                ExitCharacter(node);
                break;
            default:
                break;
        }
    }

    GameObject GenerateCharacterObject(CharacterManager.CharacterName characterName, int bodyIndex, int armIndex, int headIndex)
    {
        CharacterSO newCharacter = CharacterManager.Get().GetCharacterSO(characterName);

        Vector2 position = new Vector2(initialX, 0f);
        GameObject go = Instantiate(characterPrefab, position, Quaternion.identity, characterContainer);
        go.name = newCharacter.nameText;

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = newCharacter.bodySprites[bodyIndex];

        if (newCharacter.armSprites.Count > 0)
        {
            sr = go.transform.GetChild(0).GetComponent<SpriteRenderer>();
            sr.sprite = newCharacter.armSprites[armIndex];
        }
        else
            go.transform.GetChild(0).gameObject.SetActive(false);

        if (newCharacter.headSprites.Count > 0)
        {
            sr = go.transform.GetChild(1).GetComponent<SpriteRenderer>();
            sr.sprite = newCharacter.headSprites[headIndex];
        }
        else
            go.transform.GetChild(1).gameObject.SetActive(false);

        return go;
    }

    #region Enter/Exit Character
    void EnterCharacter(CustomCharacterActionNode node)
    {
        Character newCharacter = new Character(node.bodyIndex, node.armIndex, node.headIndex, node.character);
        GameObject characterObject = GenerateCharacterObject(node.character, node.bodyIndex, node.armIndex, node.headIndex);
        charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

        float spacing = (screenBounds.x * 2f) / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = screenBounds.x - spacing * (i + 1);

            switch (node.action)
            {
                case Action.EnterScene:
                    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, node.action));
                    activeCorroutines++;

                    break;

                case Action.PopIntoScene:
                    Vector2 position = charactersInScene[i].Value.transform.position;
                    position.x = targetX;
                    charactersInScene[i].Value.transform.position = position;

                    break;

                case Action.FadeIntoScene:
                    if (i < charactersInScene.Count - 1)
                    {
                        StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, node.action));
                        activeCorroutines++;
                    }
                    else
                    {
                        position = charactersInScene[i].Value.transform.position;
                        position.x = targetX;
                        charactersInScene[i].Value.transform.position = position;

                        SpriteRenderer sr = charactersInScene[i].Value.GetComponent<SpriteRenderer>();
                        fxManager.StartAlphaLerp0To1(sr, fadeDuration, FinishCorroutine);
                        activeCorroutines++;
                    }

                    break;

                default:
                    Debug.LogError("Cannot enter scene using selected action");
                    break;
            }
        }

        if (node.action == Action.PopIntoScene) End();
    }

    void EnterCharacter(Character character, GameObject characterObject, Action action)
    {
        Character newCharacter = new Character(character.bodyIndex, character.armIndex, character.headIndex, character.characterName);
        charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

        float spacing = (screenBounds.x * 2f) / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = screenBounds.x - spacing * (i + 1);

            switch (action)
            {
                case Action.EnterScene:
                    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, action));
                    activeCorroutines++;

                    break;

                case Action.PopIntoScene:
                    Vector2 position = charactersInScene[i].Value.transform.position;
                    position.x = targetX;
                    charactersInScene[i].Value.transform.position = position;

                    break;

                case Action.FadeIntoScene:
                    if (i < charactersInScene.Count - 1)
                    {
                        StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, action));
                        activeCorroutines++;
                    }
                    else
                    {
                        position = charactersInScene[i].Value.transform.position;
                        position.x = targetX;
                        charactersInScene[i].Value.transform.position = position;

                        SpriteRenderer sr = charactersInScene[i].Value.GetComponent<SpriteRenderer>();
                        fxManager.StartAlphaLerp0To1(sr, fadeDuration, FinishCorroutine);
                        activeCorroutines++;
                    }

                    break;

                default:
                    Debug.LogError("Cannot enter scene using selected action");
                    break;
            }
        }

        if (action == Action.PopIntoScene) End();
    }
    
    void ExitCharacter(CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<Character, GameObject> character in charactersInScene)
        {
            if (character.Key.characterName == node.character)
            {
                float targetX = screenBounds.x - initialX;

                switch (node.action)
                {
                    case Action.ExitScene:
                        StartCoroutine(MoveCharacter(character.Value.transform, targetX, false, node.action));
                        activeCorroutines++;
                        break;

                    case Action.PopOutOfScene:
                        Destroy(character.Value);
                        break;

                    case Action.FadeOutOfScene:
                        SpriteRenderer sr = character.Value.GetComponent<SpriteRenderer>();
                        fxManager.StartAlphaLerp1To0(sr, fadeDuration, FinishCorroutine);
                        activeCorroutines++;

                        fadingOutOfScene = true;
                        break;

                    default:
                        Debug.LogError("Cannot exit scene using selected action");
                        break;
                }

                charactersInScene.Remove(character);

                characterFound = true;
                break;
            }
        }
        if (!characterFound) Debug.LogError("Character not found");

        float spacing = (screenBounds.x * 2f) / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = screenBounds.x - spacing * (i + 1);

            switch (node.action)
            {
                case Action.ExitScene:
                case Action.FadeOutOfScene:
                    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, node.action));
                    activeCorroutines++;

                    break;

                case Action.PopOutOfScene:
                    Vector2 position = charactersInScene[i].Value.transform.position;
                    position.x = targetX;
                    charactersInScene[i].Value.transform.position = position;

                    break;

                default:
                    Debug.LogError("Cannot exit scene using selected action");
                    break;
            }
        }

        if (node.action == Action.PopOutOfScene) End();
    }
#endregion

    void ChangeBodyPart(BodyPart bodyPart, CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<Character, GameObject> characterInScene in charactersInScene)
        {
            if (characterInScene.Key.characterName == node.character)
            {
                CharacterSO character = CharacterManager.Get().GetCharacterSO(node.character);

                SpriteRenderer sr = null;

                switch (bodyPart)
                {
                    case BodyPart.Body:
                        if (node.bodyIndex >= 0 && node.bodyIndex < character.bodySprites.Count)
                        {
                            sr = characterInScene.Value.GetComponent<SpriteRenderer>();
                            sr.sprite = character.bodySprites[node.bodyIndex];
                        }

                        break;

                    case BodyPart.Arm:
                        if (node.armIndex >= 0 && node.armIndex < character.armSprites.Count
                            &&
                            character.armSprites.Count > 0)
                        {
                            sr = characterInScene.Value.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            sr.sprite = character.armSprites[node.armIndex];
                        }
                        else Debug.LogError("Sprite index out of range");

                        break;

                    case BodyPart.Head:
                        if (node.headIndex >= 0 && node.headIndex < character.headSprites.Count
                            &&
                            character.headSprites.Count > 0)
                        {
                            sr = characterInScene.Value.transform.GetChild(1).GetComponent<SpriteRenderer>();
                            sr.sprite = character.headSprites[node.headIndex];
                        }
                        else Debug.LogError("Sprite index out of range");

                        break;

                    default:
                        break;
                }

                characterFound = true;
                break;
            }
        }
        if (!characterFound) Debug.LogError("Character not found");

        End();
    }

    void FinishCorroutine()
    {
        activeCorroutines--;
        if (activeCorroutines <= 0) End();
    }

    void End()
    {
        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomCharacterActionNode;

        Begin(node);
    }

    public void SetLoadedData(SaveData loadedData)
    {
        if (loadedData.charactersInScene != null && loadedData.charactersInScene.Count > 0)
        {
            float spacing = (screenBounds.x * 2f) / (loadedData.charactersInScene.Count + 1);
            for (int i = 0; i < loadedData.charactersInScene.Count; i++)
            {
                int bodyIndex = loadedData.charactersInScene[i].bodyIndex;
                int armIndex = loadedData.charactersInScene[i].armIndex;
                int headIndex = loadedData.charactersInScene[i].headIndex;
                CharacterManager.CharacterName characterName = loadedData.charactersInScene[i].characterName;

                Character newCharacter = new Character(bodyIndex, armIndex, headIndex, characterName);
                GameObject characterObject = GenerateCharacterObject(characterName, bodyIndex, armIndex, headIndex);
                charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

                float targetX = screenBounds.x - spacing * (i + 1);
                Vector2 position = characterObject.transform.position;
                position.x = targetX;
                characterObject.transform.position = position;
            }
        }
    }

    IEnumerator MoveCharacter(Transform character, float targetX, bool destroyOnFinish, Action action)
    {
        float a = character.position.x;
        float b = targetX;

        float movementLength = Mathf.Abs(a - b);
        float fractionMoved = 0f;

        if (action == Action.FadeOutOfScene)
            yield return new WaitWhile(() => fadingOutOfScene == true);

        while (character.position.x < b - movementAccuracyRange / 2f || character.position.x > b + movementAccuracyRange / 2f)
        {
            float fractionToMove = (movementLength * Time.deltaTime / movementDuration) / movementLength;

            Vector2 position = character.position;
            position.x = Mathf.Lerp(a, b, fractionMoved + fractionToMove);
            character.position = position;

            fractionMoved += fractionToMove;

            yield return null;
        }

        if (destroyOnFinish) Destroy(character.gameObject);
        FinishCorroutine();
    }
}