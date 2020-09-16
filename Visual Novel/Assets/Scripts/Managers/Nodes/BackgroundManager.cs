using System;
using UnityEngine;
using nullbloq.Noodles;
using System.Collections.Generic;

[Serializable]
public class BackgroundManager : MonoBehaviour
{
    public enum Background
    {
        Forest,
        Village,
        Battlefield,
        ProtagonistHome
    }

    float cameraHeight;

    Vector2 cameraSize;

    public Sprite initialBackground;
    public GameObject bgContainer;
    SpriteRenderer bgSpriteRenderer;

    public List<BackgroundSO> backgroundPool;
    Dictionary<Background, BackgroundSO> backgrounds = new Dictionary<Background, BackgroundSO>();

    public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        bgSpriteRenderer = bgContainer.GetComponent<SpriteRenderer>();

        foreach (BackgroundSO background in backgroundPool)
        {
            backgrounds.Add(background.background, background);
        }
    }

    void OnEnable()
    {
        NoodleManager.OnBackgroundChange += ChangeBackground;
    }

    void Start()
    {
        SetBackground(initialBackground);
    }

    void OnDisable()
    {
        NoodleManager.OnBackgroundChange -= ChangeBackground;
    }

    void SetBackground(Sprite newBG)
    {
        bgSpriteRenderer.sprite = newBG;

        bgContainer.transform.localScale = Vector3.one;
        Vector2 spriteSize = bgSpriteRenderer.sprite.bounds.size;
        bgContainer.transform.localScale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;
    }

    void ChangeBackground(CustomBackgroundChangeNode node)
    {
        Background key = node.background;
        if (backgrounds.TryGetValue(key, out BackgroundSO newBG))
            SetBackground(newBG.sprite);

        OnNodeExecutionCompleted(0);
    }
}