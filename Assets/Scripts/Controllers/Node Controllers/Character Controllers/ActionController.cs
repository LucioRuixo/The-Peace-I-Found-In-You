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

    public override System.Type NodeType { protected set; get; }

    int pendingCorroutines;

    float initialX;

    [SerializeField] GameObject characterPrefab = null;
    [SerializeField] Transform characterContainer = null;
    CharacterController characterManager;

    [Header("Enter/Exit Scene: ")]
    [SerializeField] float movementDuration = 1f;
    [SerializeField] float movementAccuracyRange = 1f;

    [Header("Fade Into/Off Scene: ")]
    [SerializeField] float fadeDuration = 1f;

    bool fadingOutOfScene = false;

    List<KeyValuePair<CharacterController.Character, GameObject>> charactersInScene = new List<KeyValuePair<CharacterController.Character, GameObject>>();

    void Awake()
    {
        NodeType = typeof(CustomCharacterActionNode);

        initialX = -(characterPrefab.GetComponent<RectTransform>().rect.width / 2f);

        characterManager = transform.parent.GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        //NodeManager.OnCharacterAction += Begin;
    }

    void OnDisable()
    {
        //NodeManager.OnCharacterAction -= Begin;
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
        GameObject newCharacter = GenerateNewCharacter(node);
        charactersInScene.Add(new KeyValuePair<CharacterController.Character, GameObject>(node.character, newCharacter));

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);

            switch (node.action)
            {
                case Action.EnterScene:
                    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, node));
                    pendingCorroutines++;

                    break;

                case Action.PopIntoScene:
                    Vector2 position = charactersInScene[i].Value.transform.position;
                    position.x = targetX;
                    charactersInScene[i].Value.transform.position = position;

                    break;

                case Action.FadeIntoScene:
                    if (i < charactersInScene.Count - 1)
                    {
                        StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, node));
                        pendingCorroutines++;
                    }
                    else
                    {
                        position = charactersInScene[i].Value.transform.position;
                        position.x = targetX;
                        charactersInScene[i].Value.transform.position = position;

                        Image image = charactersInScene[i].Value.GetComponent<Image>();
                        StartCoroutine(IncreaseAlpha(image));
                        pendingCorroutines++;
                    }

                    break;

                default:
                    Debug.LogError("Cannot enter scene using selected action");
                    break;
            }

            //if (node.action == Action.EnterScene)
            //{
            //    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false));
            //    pendingCorroutines++;
            //}
            //else
            //{
            //    Vector2 position = charactersInScene[i].Value.transform.position;
            //    position.x = targetX;
            //    charactersInScene[i].Value.transform.position = position;
            //}
        }

        if (node.action == Action.PopIntoScene) End();
    }

    GameObject GenerateNewCharacter(CustomCharacterActionNode node)
    {
        CharacterSO newCharacter;
        characterManager.characterDictionary.TryGetValue(node.character, out newCharacter);

        Vector2 position = new Vector2(initialX, 0f);
        GameObject go = Instantiate(characterPrefab, position, Quaternion.identity, characterContainer);
        go.name = newCharacter.characterName;

        Image image = go.GetComponent<Image>();
        image.sprite = newCharacter.bodySprites[node.bodyIndex];
        image.SetNativeSize();
        position = new Vector2(image.rectTransform.anchoredPosition.x, 0f);
        image.rectTransform.anchoredPosition = position;

        if (newCharacter.armSprites.Count > 0)
        {
            image = go.transform.GetChild(0).GetComponent<Image>();
            image.sprite = newCharacter.armSprites[node.armIndex];
        }
        else
            go.transform.GetChild(0).gameObject.SetActive(false);

        if (newCharacter.headSprites.Count > 0)
        {
            image = go.transform.GetChild(1).GetComponent<Image>();
            image.sprite = newCharacter.headSprites[node.headIndex];
        }
        else
            go.transform.GetChild(1).gameObject.SetActive(false);

        return go;
    }
    
    void ExitCharacter(CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<CharacterController.Character, GameObject> character in charactersInScene)
        {
            if (character.Key == node.character)
            {
                float targetX = Screen.width - initialX;

                switch (node.action)
                {
                    case Action.ExitScene:
                        StartCoroutine(MoveCharacter(character.Value.transform, targetX, false, node));
                        pendingCorroutines++;

                        break;

                    case Action.PopOutOfScene:
                        Destroy(character.Value);
                        break;

                    case Action.FadeOutOfScene:
                        //Vector2 position = character.Value.transform.position;
                        //position.x = targetX;
                        //character.Value.transform.position = position;

                        Image image = character.Value.GetComponent<Image>();
                        StartCoroutine(DecreaseAlpha(image, node));
                        pendingCorroutines++;
                        fadingOutOfScene = true;

                        break;

                    default:
                        Debug.LogError("Cannot exit scene using selected action");
                        break;
                }

                //if (node.action == Action.ExitScene)
                //{
                //    StartCoroutine(MoveCharacter(characterInScene.Value.transform, targetX, true));
                //    pendingCorroutines++;
                //}
                //else Destroy(characterInScene.Value);

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
                    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false, node));
                    pendingCorroutines++;

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

            //if (node.action == Action.ExitScene)
            //{
            //    StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false));
            //    pendingCorroutines++;
            //}
            //else
            //{
            //    Vector2 position = charactersInScene[i].Value.transform.position;
            //    position.x = targetX;
            //    charactersInScene[i].Value.transform.position = position;
            //}
        }

        if (node.action == Action.PopOutOfScene) End();
    }

    void FinishCorroutine()
    {
        pendingCorroutines--;
        if (pendingCorroutines == 0) End();
    }

    void ChangeBodyPart(BodyPart bodyPart, CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<CharacterController.Character, GameObject> characterInScene in charactersInScene)
        {
            if (characterInScene.Key == node.character)
            {
                CharacterSO character;
                characterManager.characterDictionary.TryGetValue(node.character, out character);

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
        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomCharacterActionNode;

        Begin(node);
    }

    IEnumerator MoveCharacter(Transform character, float targetX, bool destroyOnFinish, CustomCharacterActionNode node)
    {
        float a = character.position.x;
        float b = targetX;

        float movementLength = Mathf.Abs(a - b);
        float fractionMoved = 0f;

        if (node.action == Action.FadeOutOfScene)
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

    IEnumerator IncreaseAlpha(Image image)
    {
        float currentAlphaValue = 0f;

        while (currentAlphaValue < 1f)
        {
            float addedValue = Time.deltaTime / fadeDuration;
            //float addedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue += addedValue;

            Color newColor = image.color;
            newColor.a = currentAlphaValue;
            image.color = newColor;

            yield return null;
        }

        End();
    }

    IEnumerator DecreaseAlpha(Image image, CustomCharacterActionNode node)
    {
        float currentAlphaValue = 1f;

        while (currentAlphaValue > 0f)
        {
            float subtractedValue = Time.deltaTime / fadeDuration;
            currentAlphaValue -= subtractedValue;

            Color newColor = image.color;
            newColor.a = currentAlphaValue;
            image.color = newColor;

            yield return null;
        }

        if (node.action == Action.FadeOutOfScene)
        {
            fadingOutOfScene = false;
        }

        End();
    }
}