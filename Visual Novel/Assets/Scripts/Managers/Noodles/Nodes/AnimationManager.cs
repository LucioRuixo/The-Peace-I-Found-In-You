using System;
using System.Collections;
using UnityEngine;
using nullbloq.Noodles;

public class AnimationManager : MonoBehaviour
{
    public enum Animation
    {
        FadeIn,
        FadeInWithBlink,
        FadeToBlack,
        FadeToBlackWithBlink,
        FadeToWhite,
        EnterFromLeft,
        LeaveOnRight,
        TopToBottom,
        BackgroundTransition,
        Blinking,
        Rain,
        Jump,
        ScreenShake
    }

    public SpriteRenderer blackSR;
    public SpriteRenderer whiteSR;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnAnimation += Begin;
    }

    void OnDisable()
    {
        NoodleManager.OnAnimation -= Begin;
    }

    void Begin(CustomAnimationNode node)
    {
        switch (node.animation)
        {
            case Animation.FadeIn:
                StartFadeIn();
                break;
            case Animation.FadeInWithBlink:
                break;
            case Animation.FadeToBlack:
                StartFadeToBlack();
                break;
            case Animation.FadeToBlackWithBlink:
                break;
            case Animation.FadeToWhite:
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
            case Animation.ScreenShake:
                break;
            default:
                Debug.LogError("Animation in node not found");
                break;
        }
    }

    void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    void StartFadeToBlack()
    {
        StartCoroutine(FadeToBlack());
    }

    void End()
    {
        OnNodeExecutionCompleted(0);
    }

    IEnumerator FadeIn()
    {
        float fadeDuration = 1f;
        float currentAlphaValue = 1f;

        while (currentAlphaValue > 0f)
        {
            float subtractedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue -= subtractedValue;

            Color newColor = blackSR.color;
            newColor.a = currentAlphaValue;
            blackSR.color = newColor;

            yield return null;
        }

        End();
    }

    IEnumerator FadeToBlack()
    {
        float fadeDuration = 1f;
        float currentAlphaValue = 0f;

        while (currentAlphaValue < 1f)
        {
            float addedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue += addedValue;

            Color newColor = blackSR.color;
            newColor.a = currentAlphaValue;
            blackSR.color = newColor;

            yield return null;
        }

        End();
    }
}