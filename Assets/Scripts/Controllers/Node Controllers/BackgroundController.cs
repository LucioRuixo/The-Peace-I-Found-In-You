using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

[Serializable]
public class BackgroundController : NodeController
{
    public enum BackgroundType
    {
        Location,
        Ilustration
    }

    public enum Location
    {
        Empty,
        Forest,
        Village,
        Battlefield,
        ProtagonistHome,
        Sanctuary,
        SeijunHome,
        BlackBackground,
        BattleForest
    }

    public enum Ilustration
    {
        Empty,
        HoshiG_A11,
        HoshiG_A12,
        HoshiB_A11,
        HoshiB_A12,
        SeijunG_A11,
        SeijunG_A12,
        SeijunB_A11,
        SeijunB_A12,
    }

    public override Type NodeType { protected set; get; }

    float cameraHeight;

    Vector2 cameraSize;

    public Sprite initialBackground;
    public GameObject bgContainer;
    SpriteRenderer bgSpriteRenderer;

    public List<BackgroundSO> locations;
    public List<BackgroundSO> ilustrations;

    void Awake()
    {
        NodeType = typeof(CustomBackgroundChangeNode);

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        bgSpriteRenderer = bgContainer.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetBackground(initialBackground);
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
        if (node.backgroundType == BackgroundType.Location)
        {
            foreach (BackgroundSO background in locations)
            {
                if (node.location == background.location)
                {
                    SetBackground(background.sprite);
                    break;
                }
            }
        }
        else
        {
            foreach (BackgroundSO background in ilustrations)
            {
                if (node.ilustration == background.ilustration)
                {
                    SetBackground(background.sprite);
                    break;
                }
            }
        }

        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomBackgroundChangeNode;

        ChangeBackground(node);
    }
}