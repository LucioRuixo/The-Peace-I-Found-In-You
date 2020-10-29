using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using nullbloq.Noodles;

/*
DUDAS:
- Los pjs se siguen viendo durante las decisiones? (POR AHORA: sí)
- Siempre van a entrar por la izquierda e irse por la derecha? (POR AHORA: sí)
- Pueden entrar o irse más de un pj a la vez? (POR AHORA: no)
- Si un pj entra por la izquierda, se queda en la posición más a la izquierda o puede ponerse en otra? (POR AHORA: se queda en la izquierda)
- Pueden cambiar de posición? (POR AHORA: no)
- Hay márgenes a los costados de la pantalla a tener en cuenta al posicionar a los pjs? (POR AHORA: no)
*/
public class ActionController : NodeController
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

    float initialX;

    [SerializeField] GameObject characterPrefab = null;
    [SerializeField] Transform characterContainer = null;
    FXManager fxManager;

    List<KeyValuePair<Character, GameObject>> charactersInScene = new List<KeyValuePair<Character, GameObject>>();

    [Header("Enter/Exit Scene: ")]
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

        initialX = -(characterPrefab.GetComponent<RectTransform>().rect.width / 2f);
        fxManager = FXManager.Get();
    }

    void Update()
    {
        Debug.Log("pending corroutines: " + activeCorroutines);
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

    void EnterCharacter(CustomCharacterActionNode node)
    {
        Character newCharacter = new Character(node.bodyIndex, node.armIndex, node.headIndex, node.character);
        GameObject characterObject = GenerateCharacterObject(node.character, node.bodyIndex, node.armIndex, node.headIndex);
        charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);

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

                        Image image = charactersInScene[i].Value.GetComponent<Image>();
                        fxManager.StartAlphaLerp0To1(image, fadeDuration, FinishCorroutine);
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
        Character newCharacter = new Character(character.BodyIndex, character.ArmIndex, character.HeadIndex, character.CharacterName);
        charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);

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

                        Image image = charactersInScene[i].Value.GetComponent<Image>();
                        fxManager.StartAlphaLerp0To1(image, fadeDuration, FinishCorroutine);
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

    GameObject GenerateCharacterObject(CharacterManager.CharacterName characterName, int bodyIndex, int armIndex, int headIndex)
    {
        CharacterSO newCharacter = CharacterManager.Get().GetCharacterSO(characterName);

        Vector2 position = new Vector2(initialX, 0f);
        GameObject go = Instantiate(characterPrefab, position, Quaternion.identity, characterContainer);
        go.name = newCharacter.nameText;

        Image image = go.GetComponent<Image>();
        image.sprite = newCharacter.bodySprites[bodyIndex];
        image.SetNativeSize();
        position = new Vector2(image.rectTransform.anchoredPosition.x, 0f);
        image.rectTransform.anchoredPosition = position;

        if (newCharacter.armSprites.Count > 0)
        {
            image = go.transform.GetChild(0).GetComponent<Image>();
            image.sprite = newCharacter.armSprites[armIndex];
        }
        else
            go.transform.GetChild(0).gameObject.SetActive(false);

        if (newCharacter.headSprites.Count > 0)
        {
            image = go.transform.GetChild(1).GetComponent<Image>();
            image.sprite = newCharacter.headSprites[headIndex];
        }
        else
            go.transform.GetChild(1).gameObject.SetActive(false);

        return go;
    }
    
    void ExitCharacter(CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<Character, GameObject> character in charactersInScene)
        {
            if (character.Key.CharacterName == node.character)
            {
                float targetX = Screen.width - initialX;

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
                        Image image = character.Value.GetComponent<Image>();
                        fxManager.StartAlphaLerp1To0(image, fadeDuration, FinishCorroutine);
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

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);

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

    void FinishCorroutine()
    {
        Debug.Log("finishing corroutine");
        activeCorroutines--;
        if (activeCorroutines <= 0) End();
    }

    void ChangeBodyPart(BodyPart bodyPart, CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<Character, GameObject> characterInScene in charactersInScene)
        {
            if (characterInScene.Key.CharacterName == node.character)
            {
                CharacterSO character = CharacterManager.Get().GetCharacterSO(node.character);

                Image image = null;

                switch (bodyPart)
                {
                    case BodyPart.Body:
                        if (node.bodyIndex >= 0 && node.bodyIndex < character.bodySprites.Count)
                        {
                            image = characterInScene.Value.GetComponent<Image>();
                            image.sprite = character.bodySprites[node.bodyIndex];
                        }

                        break;

                    case BodyPart.Arm:
                        if (node.armIndex >= 0 && node.armIndex < character.armSprites.Count
                            &&
                            character.armSprites.Count > 0)
                        {
                            image = characterInScene.Value.transform.GetChild(0).GetComponent<Image>();
                            image.sprite = character.armSprites[node.armIndex];
                        }
                        else Debug.LogError("Sprite index out of range");

                        break;

                    case BodyPart.Head:
                        if (node.headIndex >= 0 && node.headIndex < character.headSprites.Count
                            &&
                            character.headSprites.Count > 0)
                        {
                            image = characterInScene.Value.transform.GetChild(1).GetComponent<Image>();
                            image.sprite = character.headSprites[node.headIndex];
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

    void End()
    {
        Debug.Log("ending character action");
        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomCharacterActionNode;

        Begin(node);
    }

    public void SetData(GameManager.GameData loadedData)
    {
        if (loadedData.charactersInScene != null && loadedData.charactersInScene.Count > 0)
        {
            float spacing = Screen.width / (loadedData.charactersInScene.Count + 1);
            for (int i = 0; i < loadedData.charactersInScene.Count; i++)
            {
                int bodyIndex = loadedData.charactersInScene[i].BodyIndex;
                int armIndex = loadedData.charactersInScene[i].ArmIndex;
                int headIndex = loadedData.charactersInScene[i].HeadIndex;
                CharacterManager.CharacterName characterName = loadedData.charactersInScene[i].CharacterName;

                Character newCharacter = new Character(bodyIndex, armIndex, headIndex, characterName);
                GameObject characterObject = GenerateCharacterObject(characterName, bodyIndex, armIndex, headIndex);
                charactersInScene.Add(new KeyValuePair<Character, GameObject>(newCharacter, characterObject));

                float targetX = Screen.width - spacing * (i + 1);
                Vector2 position = characterObject.transform.position;
                position.x = targetX;
                characterObject.transform.position = position;
            }
        }
    }

    //public List<Character> GetCharactersInScene()
    //{
    //    List<Character> characterKeys = new List<Character>();
    //    foreach (KeyValuePair<Character, GameObject> character in charactersInScene)
    //    {
    //        characterKeys.Add(character.Key);
    //    }
    //
    //    return characterKeys;
    //}

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