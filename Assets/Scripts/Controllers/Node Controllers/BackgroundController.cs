using System;
using System.Collections.Generic;
using UnityEngine;
using nullbloq.Noodles;

[Serializable]
public class BackgroundController : NodeController, ISaveComponent
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

    public enum BackgroundFX
    {
        None,
        Leaves,
        YellowLightBugs,
        BlueLightBugs,
        RandomYellowLightBugs,
        RandomBlueLightBugs
    }

    public override Type NodeType { protected set; get; }

    float cameraHeight;

    Vector2 cameraSize;

    SaveData.BackgroundData currentBackgroundData;

    [SerializeField] GameObject backgroundContainer = null;
    SpriteRenderer backgroundSR;

    [SerializeField] List<BackgroundSO> locations = null;
    [SerializeField] List<BackgroundSO> ilustrations = null;

    [Header("Background FX: ")]
    [SerializeField] Leaves leaves = null;
    [SerializeField] FireflyManager lightBugs = null;

    public SaveData.BackgroundData CurrentBackgroundData { get { return currentBackgroundData; } }

    void Awake()
    {
        NodeType = typeof(CustomBackgroundChangeNode);

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        backgroundSR = backgroundContainer.GetComponent<SpriteRenderer>();
    }

    BackgroundSO GetBackgroundSprite(BackgroundType type, Location location, Ilustration ilustration)
    {
        if (type == BackgroundType.Location)
        {
            foreach (BackgroundSO background in locations)
            {
                if (location == background.location)
                    return background;
            }
        }
        else
        {
            foreach (BackgroundSO background in ilustrations)
            {
                if (ilustration == background.ilustration)
                    return background;
            }
        }

        return null;
    }

    void SetBackground(BackgroundType type, Location location, Ilustration ilustration)
    {
        if (leaves.IsPlaying) leaves.Stop();
        if (lightBugs.IsPlaying) lightBugs.Stop();

        BackgroundSO newBackground = GetBackgroundSprite(type, location, ilustration);
        backgroundSR.sprite = newBackground.sprite;

        backgroundContainer.transform.localScale = Vector3.one;
        Vector2 spriteSize = backgroundSR.sprite.bounds.size;
        backgroundContainer.transform.localScale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;

        if (newBackground.effect != BackgroundFX.None)
        {
            switch (newBackground.effect)
            {
                case BackgroundFX.Leaves:
                    leaves.Play();
                    break;
                case BackgroundFX.YellowLightBugs:
                    lightBugs.Play(FireflyManager.FireflyColor.Yellow, false);
                    break;
                case BackgroundFX.BlueLightBugs:
                    lightBugs.Play(FireflyManager.FireflyColor.Blue, false);
                    break;
                case BackgroundFX.RandomYellowLightBugs:
                    lightBugs.Play(FireflyManager.FireflyColor.Yellow, true);
                    break;
                case BackgroundFX.RandomBlueLightBugs:
                    lightBugs.Play(FireflyManager.FireflyColor.Blue, true);
                    break;
                default:
                    break;
            }
        }

        currentBackgroundData = new SaveData.BackgroundData(type, location, ilustration);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomBackgroundChangeNode;

        SetBackground(node.backgroundType, node.location, node.ilustration);

        CallNodeExecutionCompletion(0);
    }

    public void SetLoadedData(SaveData loadedData)
    {
        SetBackground(loadedData.backgroundData.type, loadedData.backgroundData.location, loadedData.backgroundData.ilustration);
    }
}