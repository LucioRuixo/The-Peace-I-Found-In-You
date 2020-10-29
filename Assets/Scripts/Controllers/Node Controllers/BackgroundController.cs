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
        Forest_Day,
        Village,
        Battlefield,
        ProtagonistHome_Day,
        Sanctuary_Day,
        SeijunHome,
        BlackBackground,
        BattleForest,
        Forest_Night,
        ProtagonistHome_Night,
        Sanctuary_Night
    }

    public enum Ilustration
    {
        Empty,
        HoshiG_A11,
        HoshiG_A12_1,
        HoshiG_A12_2,
        HoshiB_A11,
        HoshiB_A12,
        SeijunG_A11,
        SeijunG_A12,
        SeijunB_A11,
        SeijunB_A12,
    }

    [Serializable]
    public struct BackgroundData
    {
        public BackgroundType type;
        public Location location;
        public Ilustration ilustration;

        public BackgroundData(BackgroundType _type, Location _location, Ilustration _ilustration)
        {
            type = _type;
            location = _location;
            ilustration = _ilustration;
        }
    }

    public override Type NodeType { protected set; get; }

    float cameraHeight;

    Vector2 cameraSize;

    BackgroundData currentBackgroundData;

    [SerializeField] GameObject backgroundContainer = null;
    SpriteRenderer backgroundSR;

    [SerializeField] List<BackgroundSO> locations = null;
    [SerializeField] List<BackgroundSO> ilustrations = null;

    public BackgroundData CurrentBackgroundData { get { return currentBackgroundData; } }

    void Awake()
    {
        NodeType = typeof(CustomBackgroundChangeNode);

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        backgroundSR = backgroundContainer.GetComponent<SpriteRenderer>();
    }

    Sprite GetBackgroundSprite(BackgroundType type, Location location, Ilustration ilustration)
    {
        if (type == BackgroundType.Location)
        {
            foreach (BackgroundSO background in locations)
            {
                if (location == background.location)
                    return background.sprite;
            }
        }
        else
        {
            foreach (BackgroundSO background in ilustrations)
            {
                if (ilustration == background.ilustration)
                    return background.sprite;
            }
        }

        return null;
    }

    void SetBackground(BackgroundType type, Location location, Ilustration ilustration)
    {
        Sprite newBackground = GetBackgroundSprite(type, location, ilustration);
        backgroundSR.sprite = newBackground;

        backgroundContainer.transform.localScale = Vector3.one;
        Vector2 spriteSize = backgroundSR.sprite.bounds.size;
        backgroundContainer.transform.localScale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;

        currentBackgroundData = new BackgroundData(type, location, ilustration);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomBackgroundChangeNode;

        SetBackground(node.backgroundType, node.location, node.ilustration);

        CallNodeExecutionCompletion(0);
    }

    public void SetData(GameManager.GameData loadedData)
    {
        SetBackground(loadedData.backgroundData.type, loadedData.backgroundData.location, loadedData.backgroundData.ilustration);
    }
}