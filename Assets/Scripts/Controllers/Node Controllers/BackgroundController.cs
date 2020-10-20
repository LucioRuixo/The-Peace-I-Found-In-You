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
    Dictionary<Location, BackgroundSO> locationDictionary = new Dictionary<Location, BackgroundSO>();
    Dictionary<Ilustration, BackgroundSO> ilustrationDictionary = new Dictionary<Ilustration, BackgroundSO>();

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomBackgroundChangeNode);

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        bgSpriteRenderer = bgContainer.GetComponent<SpriteRenderer>();

        foreach (BackgroundSO location in locations)
        {
            locationDictionary.Add(location.location, location);
        }

        foreach (BackgroundSO ilustration in ilustrations)
        {
            ilustrationDictionary.Add(ilustration.ilustration, ilustration);
        }
    }

    void OnEnable()
    {
        //NodeManager.OnBackgroundChange += ChangeBackground;
    }

    void Start()
    {
        SetBackground(initialBackground);
    }

    void OnDisable()
    {
        //NodeManager.OnBackgroundChange -= ChangeBackground;
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
            Location key = node.location;
            if (locationDictionary.TryGetValue(key, out BackgroundSO newBG))
                SetBackground(newBG.sprite);
        }
        else
        {
            Ilustration key = node.ilustration;
            if (ilustrationDictionary.TryGetValue(key, out BackgroundSO newBG))
                SetBackground(newBG.sprite);
        }

        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomBackgroundChangeNode;

        ChangeBackground(node);
    }
}