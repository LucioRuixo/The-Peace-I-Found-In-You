using System;
using UnityEngine;
using UnityEngine.UI;
using nullbloq.Noodles;

public class AnimationController : NodeController
{
    public enum Animation
    {
        FadeToBlack,
        FadeFromBlack,
        FadeToWhite,
        FadeFromWhite,
        BlinkOpen,
        BlinkClosed,
        BackgroundTransition,
        Blinking,
        CameraShake,
    }

    public override Type NodeType { protected set; get; }

    [SerializeField] float fadeDuration = 1f;

    [SerializeField] new GameObject animation = null;
    [SerializeField] Image blackCover = null;
    [SerializeField] Image whiteCover = null;
    [SerializeField] Animator fadeInBlinkTop = null, fadeInBlinkBottom = null;
    FXManager fxManager;

    [Header("Blink Animation: ")]
    [SerializeField] string blinkOpenTrigger = "";

    [Header("Camera Shake: ")]
    [SerializeField] int shakePointsAmount = 1;
    [SerializeField] float shakeMagnitude = 1f;
    [SerializeField] float shakeSpeed = 1f;

    void Awake()
    {
        NodeType = typeof(CustomAnimationNode);

        fxManager = FXManager.Get();
    }

    void OnEnable()
    {
        Blink.OnBlinkOpenCompleted += End;
    }

    void OnDisable()
    {
        Blink.OnBlinkOpenCompleted -= End;
    }

    void Begin(CustomAnimationNode node)
    {
        animation.SetActive(true);

        switch (node.animation)
        {
            case Animation.FadeToBlack:
                fxManager.StartAlphaLerp0To1(blackCover, fadeDuration, End);
                break;
            case Animation.FadeFromBlack:
                fxManager.StartAlphaLerp1To0(blackCover, fadeDuration, End);
                break;
            case Animation.FadeToWhite:
                fxManager.StartAlphaLerp0To1(whiteCover, fadeDuration, End);
                break;
            case Animation.FadeFromWhite:
                fxManager.StartAlphaLerp1To0(whiteCover, fadeDuration, End);
                break;
            case Animation.BlinkOpen:
                StartBlinkOpen();
                break;
            case Animation.BlinkClosed:
                // No implementado
                break;
            case Animation.BackgroundTransition:
                // No implementado
                break;
            case Animation.Blinking:
                // No implementado
                break;
            case Animation.CameraShake:
                fxManager.StartCameraShake(shakePointsAmount, shakeMagnitude, shakeSpeed, End);
                break;
            default:
                Debug.LogError("Animation in node not found");
                break;
        }
    }

    void StartBlinkOpen()
    {
        fadeInBlinkTop.SetTrigger(blinkOpenTrigger);
        fadeInBlinkBottom.SetTrigger(blinkOpenTrigger);
    }

    void End()
    {
        CallNodeExecutionCompletion(0);
    }

    public override void Execute(NoodlesNode genericNode)
    {
        var node = genericNode as CustomAnimationNode;

        Begin(node);
    }
}