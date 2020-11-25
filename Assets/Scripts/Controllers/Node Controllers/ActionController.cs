using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

public class ActionController : NodeController, ISaveComponent //TODO: simplificar esta clase (por ahí separar en CharacterSpriteController y CharacterEnterExitController y hacer nodos para cada una)
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

    public override Type NodeType { protected set; get; }

    bool fadingOutOfScene = false;

    int activeCorroutines;

    float initialX;

    [SerializeField] GameObject staticCharacterPrefab = null;
    [SerializeField] Transform characterContainer = null;
    FXManager fxManager;
    ScreenManager screenManager;

    List<KeyValuePair<Character, GameObject>> charactersInScene = new List<KeyValuePair<Character, GameObject>>();

    [Header("Enter/Exit Scene: ")]
    [SerializeField] float offScreenCharacterWidth = 1f;
    [SerializeField] float movementDuration = 1f;
    [SerializeField] float movementAccuracyRange = 1f;

    [Header("Fade Into/Off Scene: ")]
    [SerializeField] float fadeDuration = 1f;

    public List<SaveData.CharacterData> CharactersInScene { get
    {
        List<SaveData.CharacterData> charactersData = new List<SaveData.CharacterData>();
        foreach (KeyValuePair<Character, GameObject> character in charactersInScene)
        {
            SaveData.CharacterData characterData = new SaveData.CharacterData
            {
                bodyIndex = character.Key.bodyIndex,
                armIndex = character.Key.armIndex,
                headIndex = character.Key.headIndex,
                name = character.Key.characterName
            };

            charactersData.Add(characterData);
        }

        return charactersData;
    } }

    void Awake()
    {
        NodeType = typeof(CustomCharacterActionNode);

        fxManager = FXManager.Get();
        screenManager = ScreenManager.Get();

        initialX = screenManager.MinScreenLimits.x - offScreenCharacterWidth / 2f;

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
                ChangeBodyPart(Character.BodyPart.Body, node);
                break;
            case Action.ChangeArm:
                ChangeBodyPart(Character.BodyPart.Arm, node);
                break;
            case Action.ChangeHead:
                ChangeBodyPart(Character.BodyPart.Head, node);
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
        CharacterSO characterData = CharacterManager.Get().GetCharacterSO(characterName);
        Vector2 position = new Vector2(initialX, 0f);
        GameObject go = null;

        if (characterData.armatureObject)
            go = Instantiate(characterData.armatureObject, position, Quaternion.identity, characterContainer);
        else
            go = Instantiate(staticCharacterPrefab, position, Quaternion.identity, characterContainer);

        go.name = characterData.nameText;

        Character newCaracter = go.GetComponent<Character>();
        newCaracter.Initialize(bodyIndex, armIndex, headIndex, characterName);

        return go;
    }

    #region Enter/Exit Character
    void EnterCharacter(CustomCharacterActionNode node)
    {
        GameObject characterObject = GenerateCharacterObject(node.character, node.bodyIndex, node.armIndex, node.headIndex);
        Character newCharacter = characterObject.GetComponent<Character>();
        charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

        float spacing = (screenManager.ScreenBounds.x * 2f) / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = screenManager.ScreenBounds.x - spacing * (i + 1);

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
        //Character newCharacter = new Character(character.bodyIndex, character.armIndex, character.headIndex, character.characterName);
        Character newCharacter = characterObject.GetComponent<Character>();
        charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

        float spacing = (screenManager.ScreenBounds.x * 2f) / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = screenManager.ScreenBounds.x - spacing * (i + 1);

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
                float targetX = screenManager.ScreenBounds.x - initialX;

                switch (node.action)
                {
                    case Action.ExitScene:
                        StartCoroutine(MoveCharacter(character.Value.transform, targetX, true, node.action));
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

        float spacing = (screenManager.ScreenBounds.x * 2f) / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = screenManager.ScreenBounds.x - spacing * (i + 1);

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

    void ChangeBodyPart(Character.BodyPart bodyPart, CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<Character, GameObject> characterInScene in charactersInScene)
        {
            if (characterInScene.Key.characterName == node.character)
            {
                Character character = characterInScene.Value.GetComponent<Character>();

                switch (bodyPart)
                {
                    case Character.BodyPart.Body:
                        character.ChangeBodyPart(bodyPart, node.bodyIndex);
                        break;
                    case Character.BodyPart.Arm:
                        character.ChangeBodyPart(bodyPart, node.armIndex);
                        break;
                    case Character.BodyPart.Head:
                        character.ChangeBodyPart(bodyPart, node.headIndex);
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
            float spacing = (screenManager.ScreenBounds.x * 2f) / (loadedData.charactersInScene.Count + 1);
            for (int i = 0; i < loadedData.charactersInScene.Count; i++)
            {
                int bodyIndex = loadedData.charactersInScene[i].bodyIndex;
                int armIndex = loadedData.charactersInScene[i].armIndex;
                int headIndex = loadedData.charactersInScene[i].headIndex;
                CharacterManager.CharacterName characterName = loadedData.charactersInScene[i].name;

                GameObject characterObject = GenerateCharacterObject(characterName, bodyIndex, armIndex, headIndex);
                //Character newCharacter = new Character(bodyIndex, armIndex, headIndex, characterName);
                Character newCharacter = characterObject.GetComponent<Character>();
                charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

                float targetX = screenManager.ScreenBounds.x - spacing * (i + 1);
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