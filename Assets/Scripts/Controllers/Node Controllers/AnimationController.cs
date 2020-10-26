﻿using System;
using System.Collections;
using System.Collections.Generic;
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
        EnterFromLeft,
        LeaveOnRight,
        TopToBottom,
        BackgroundTransition,
        Blinking,
        Rain,
        Jump,
        CameraShake,
        AfternoonFilter,
        NightFilter,
        DisableFilter
    }

    public override Type NodeType { protected set; get; }

    [SerializeField] float fadeDuration = 1f;

    [SerializeField] new GameObject animation = null;
    [SerializeField] Image blackCover = null;
    [SerializeField] Image whiteCover = null;
    [SerializeField] Image filter = null;
    [SerializeField] Animator fadeInBlinkTop = null, fadeInBlinkBottom = null;
    FXManager fxManager;

    [Header("Camera Shake: ")]
    [SerializeField] int shakePointsAmount = 1;
    [SerializeField] float shakeMagnitude = 1f;
    [SerializeField] float shakeSpeed = 1f;

    [Header("Filters: ")]
    [SerializeField] Sprite afternoonFilter = null;
    [SerializeField] Sprite nightFilter = null;

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
                //StartCoroutine(IncreaseAlpha(blackCover));
                fxManager.StartAlphaLerp0To1(blackCover, fadeDuration, End);
                break;
            case Animation.FadeFromBlack:
                //StartCoroutine(DecreaseAlpha(blackCover));
                fxManager.StartAlphaLerp1To0(blackCover, fadeDuration, End);
                break;
            case Animation.FadeToWhite:
                //StartCoroutine(IncreaseAlpha(whiteCover));
                fxManager.StartAlphaLerp0To1(whiteCover, fadeDuration, End);
                break;
            case Animation.FadeFromWhite:
                //StartCoroutine(DecreaseAlpha(whiteCover));
                fxManager.StartAlphaLerp1To0(whiteCover, fadeDuration, End);
                break;
            case Animation.BlinkOpen:
                StartBlinkOpen();
                break;
            case Animation.BlinkClosed:
                break;
            case Animation.EnterFromLeft:
                break;
            case Animation.LeaveOnRight:
                break;
            case Animation.TopToBottom:
                break;
            case Animation.BackgroundTransition:
                break;
            case Animation.Blinking:
                break;
            case Animation.Rain:
                break;
            case Animation.Jump:
                break;
            case Animation.CameraShake:
                //StartCoroutine(ShakeCamera());
                fxManager.StartCameraShake(shakePointsAmount, shakeMagnitude, shakeSpeed, End);
                break;
            case Animation.AfternoonFilter:
                ApplyFilter(afternoonFilter);
                break;
            case Animation.NightFilter:
                ApplyFilter(nightFilter);
                break;
            case Animation.DisableFilter:
                ApplyFilter(null);
                break;
            default:
                Debug.LogError("Animation in node not found");
                break;
        }
    }

    void StartBlinkOpen()
    {
        fadeInBlinkTop.SetTrigger("Blink Open");
        fadeInBlinkBottom.SetTrigger("Blink Open");
    }

    void ApplyFilter(Sprite newFilter)
    {
        if (newFilter)
        {
            filter.sprite = newFilter;
            filter.gameObject.SetActive(true);
            End();
        }
        else
        {
            filter.gameObject.SetActive(false);
            End();
        }
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