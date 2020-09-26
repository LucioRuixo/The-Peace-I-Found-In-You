using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

/*
DUDAS:
- Los pjs se siguen viendo durante las decisiones? (POR AHORA: sí)
- Siempre van a entrar por la izquierda e irse por la derecha? (POR AHORA: sí)
- Pueden entrar o irse más de un pj a la vez? (POR AHORA: no)
- Si un pj entra por la izquierda, se queda en la posición más a la izquierda o puede ponerse en otra? (POR AHORA: se queda en la izquierda)
- Pueden cambiar de posición? (POR AHORA: no)
- Hay márgenes a los costados de la pantalla a tener en cuenta al posicionar a los pjs? (POR AHORA: no)
*/
public class ActionManager : MonoBehaviour
{
    public enum Action
    {
        EnterScene,
        ExitScene,
        ChangeBody,
        ChangeHead,
        ChangeArm
    }

    enum BodyPart
    {
        Body,
        Arm,
        Head
    }

    int pendingCorroutines;

    public float movementDuration;
    float initialX;

    public GameObject characterPrefab;
    public Transform characterContainer;
    CharacterManager characterManager;

    List<KeyValuePair<CharacterManager.Character, GameObject>> charactersInScene = new List<KeyValuePair<CharacterManager.Character, GameObject>>();

    public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        initialX = -(characterPrefab.GetComponent<RectTransform>().rect.width / 2f);

        characterManager = transform.parent.GetComponent<CharacterManager>();
    }

    void OnEnable()
    {
        NoodleManager.OnCharacterAction += Begin;
    }

    void OnDisable()
    {
        NoodleManager.OnCharacterAction -= Begin;
    }

    void Begin(CustomCharacterActionNode node)
    {
        switch (node.action)
        {
            case Action.EnterScene:
                EnterCharacter(node);
                break;
            case Action.ExitScene:
                ExitCharacter(node.character);
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
            default:
                break;
        }
    }

    void EnterCharacter(CustomCharacterActionNode node)
    {
        GameObject newCharacter = GenerateNewCharacter(node);
        charactersInScene.Add(new KeyValuePair<CharacterManager.Character, GameObject>(node.character, newCharacter));

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);
            StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false));
            pendingCorroutines++;
        }
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
            //image.SetNativeSize();
            //position = new Vector2(image.rectTransform.anchoredPosition.x, 0f);
            //image.rectTransform.anchoredPosition = position;
        }
        else
            go.transform.GetChild(0).gameObject.SetActive(false);

        if (newCharacter.headSprites.Count > 0)
        {
            image = go.transform.GetChild(1).GetComponent<Image>();
            image.sprite = newCharacter.headSprites[node.headIndex];
            //image.SetNativeSize();
            //position = new Vector2(image.rectTransform.anchoredPosition.x, 0f);
            //image.rectTransform.anchoredPosition = position;
        }
        else
            go.transform.GetChild(1).gameObject.SetActive(false);

        return go;
    }
    
    void ExitCharacter(CharacterManager.Character character)
    {
        bool characterFound = false;
        foreach (KeyValuePair<CharacterManager.Character, GameObject> characterInScene in charactersInScene)
        {
            if (characterInScene.Key == character)
            {
                float targetX = Screen.width - initialX;
                StartCoroutine(MoveCharacter(characterInScene.Value.transform, targetX, true));
                pendingCorroutines++;

                charactersInScene.Remove(characterInScene);

                characterFound = true;
                break;
            }
        }
        if (!characterFound) Debug.LogError("Character not found");

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);
            StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false));
            pendingCorroutines++;
        }
    }

    void FinishCorroutine()
    {
        pendingCorroutines--;
        if (pendingCorroutines == 0) End();
    }

    void ChangeBodyPart(BodyPart bodyPart, CustomCharacterActionNode node)
    {
        bool characterFound = false;
        foreach (KeyValuePair<CharacterManager.Character, GameObject> characterInScene in charactersInScene)
        {
            if (characterInScene.Key == node.character)
            {
                CharacterSO character;
                characterManager.characterDictionary.TryGetValue(node.character, out character);

                Image image = null;
                bool imageChanged = false;

                switch (bodyPart)
                {
                    case BodyPart.Body:
                        if (node.bodyIndex >= 0 && node.bodyIndex < character.bodySprites.Count)
                        {
                            image = characterInScene.Value.GetComponent<Image>();
                            image.sprite = character.bodySprites[node.bodyIndex];

                            imageChanged = true;
                        }
                        break;
                    case BodyPart.Arm:
                        if (node.armIndex >= 0 && node.armIndex < character.armSprites.Count
                            &&
                            character.armSprites.Count > 0)
                        {
                            image = characterInScene.Value.transform.GetChild(0).GetComponent<Image>();
                            image.sprite = character.armSprites[node.armIndex];

                            imageChanged = true;
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

                            imageChanged = true;
                        }
                        else Debug.LogError("Sprite index out of range");
                        break;
                    default:
                        break;
                }

                if (image && imageChanged)
                {
                    image.SetNativeSize();

                    Vector2 position = new Vector2(image.rectTransform.anchoredPosition.x, 0f);
                    image.rectTransform.anchoredPosition = position;
                }

                characterFound = true;
                break;
            }
        }
        if (!characterFound) Debug.LogError("Character not found");
    }

    void End()
    {
        OnNodeExecutionCompleted?.Invoke(0);
    }

    IEnumerator MoveCharacter(Transform character, float targetX, bool destroyOnFinish)
    {
        float a = character.position.x;
        float b = targetX;

        float movementLength = Mathf.Abs(a - b);
        float fractionMoved = 0f;
        while (character.position.x != b)
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