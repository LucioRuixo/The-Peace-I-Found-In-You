using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;
using System.Collections;
using UnityEngine.UI;

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
        ExitScene
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
        if (node.action == Action.EnterScene) EnterCharacter(node.character);
        else ExitCharacter(node.character);
    }

    void EnterCharacter(CharacterManager.Character character)
    {
        GameObject newCharacter = GenerateNewCharacter(character);
        charactersInScene.Add(new KeyValuePair<CharacterManager.Character, GameObject>(character, newCharacter));

        float spacing = Screen.width / (charactersInScene.Count + 1);
        for (int i = 0; i < charactersInScene.Count; i++)
        {
            float targetX = Screen.width - spacing * (i + 1);
            StartCoroutine(MoveCharacter(charactersInScene[i].Value.transform, targetX, false));
            pendingCorroutines++;
        }
    }

    GameObject GenerateNewCharacter(CharacterManager.Character character)
    {
        CharacterSO newCharacter;
        characterManager.characterDictionary.TryGetValue(character, out newCharacter);

        Vector2 position = new Vector2(initialX, 0f);
        GameObject go = Instantiate(characterPrefab, position, Quaternion.identity, characterContainer);
        go.name = newCharacter.characterName;
        go.GetComponent<Image>().sprite = newCharacter.sprite;

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