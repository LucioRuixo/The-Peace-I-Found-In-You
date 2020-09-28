using System;
using System.Collections;
using UnityEngine;
using nullbloq.Noodles;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
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
        ScreenShake
    }

    public Image blackCover;
    public Image whiteCover;
    public Animator fadeInBlinkTop, fadeInBlinkBottom;

    public static event Action<int> OnNodeExecutionCompleted;

    void OnEnable()
    {
        NoodleManager.OnAnimation += Begin;
        Blink.OnBlinkOpenCompleted += End;
    }

    void OnDisable()
    {
        NoodleManager.OnAnimation -= Begin;
        Blink.OnBlinkOpenCompleted -= End;
    }

    void Begin(CustomAnimationNode node)
    {
        switch (node.animation)
        {
            case Animation.FadeToBlack:
                StartCoroutine(IncreaseAlpha(blackCover));
                break;
            case Animation.FadeFromBlack:
                StartCoroutine(DecreaseAlpha(blackCover));
                break;
            case Animation.FadeToWhite:
                StartCoroutine(IncreaseAlpha(whiteCover));
                break;
            case Animation.FadeFromWhite:
                StartCoroutine(DecreaseAlpha(whiteCover));
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
            case Animation.ScreenShake:
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

    void End()
    {
        OnNodeExecutionCompleted(0);
    }

    IEnumerator IncreaseAlpha(Image image)
    {
        float fadeDuration = 1f;
        float currentAlphaValue = 0f;

        while (currentAlphaValue < 1f)
        {
            float addedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue += addedValue;

            Color newColor = image.color;
            newColor.a = currentAlphaValue;
            image.color = newColor;

            yield return null;
        }

        End();
    }

    IEnumerator DecreaseAlpha(Image image)
    {
        float fadeDuration = 1f;
        float currentAlphaValue = 1f;

        while (currentAlphaValue > 0f)
        {
            float subtractedValue = 1f / (fadeDuration / Time.deltaTime);
            currentAlphaValue -= subtractedValue;

            Color newColor = image.color;
            newColor.a = currentAlphaValue;
            image.color = newColor;

            yield return null;
        }

        End();
    }
}