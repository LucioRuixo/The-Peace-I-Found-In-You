using System;
using UnityEngine;
using nullbloq.Noodles;

[Serializable]
public class BackgroundManager : MonoBehaviour
{
    float cameraHeight;

    Vector2 cameraSize;

    public Sprite initialBackground;
    public GameObject bgContainer;
    SpriteRenderer bgSpriteRenderer;

    public static event Action<string> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NodeManager.OnBackgroundChange += ChangeBackground;
    }

    void Start()
    {
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        bgSpriteRenderer = bgContainer.GetComponent<SpriteRenderer>();

        SetBackground(initialBackground);
    }

    void OnDisable()
    {
        NodeManager.OnBackgroundChange -= ChangeBackground;
    }

    void SetBackground(Sprite newBG)
    {
        bgSpriteRenderer.sprite = newBG;

        bgContainer.transform.localScale = Vector3.one;
        Vector2 spriteSize = bgSpriteRenderer.sprite.bounds.size;
        bgContainer.transform.localScale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;
    }

    void ChangeBackground(NoodlesNode node)
    {
        //SetBackground(data.background);
        //
        //data.nextBit.Execute();
    }
}