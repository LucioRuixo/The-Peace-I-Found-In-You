using System;
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

    [SerializeField] new GameObject animation = null;
    [SerializeField] Image blackCover = null;
    [SerializeField] Image whiteCover = null;
    [SerializeField] Image filter = null;
    [SerializeField] Animator fadeInBlinkTop = null, fadeInBlinkBottom = null;

    [Header("Camera Shake: ")]
    [SerializeField] int shakePointsAmount = 1;
    [SerializeField] float shakeMagnitude = 1f;
    [SerializeField] float shakeSpeed = 1f;

    [Header("Filters: ")]
    [SerializeField] Sprite afternoonFilter = null;
    [SerializeField] Sprite nightFilter = null;

    //public static event Action<int> OnNodeExecutionCompleted;

    void Awake()
    {
        NodeType = typeof(CustomAnimationNode);
    }

    void OnEnable()
    {
        //NodeManager.OnAnimation += Begin;
        Blink.OnBlinkOpenCompleted += End;
    }

    void OnDisable()
    {
        //NodeManager.OnAnimation -= Begin;
        Blink.OnBlinkOpenCompleted -= End;
    }

    void Begin(CustomAnimationNode node)
    {
        animation.SetActive(true);

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
            case Animation.CameraShake:
                StartCoroutine(ShakeCamera());
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

    IEnumerator ShakeCamera()
    {
        Transform camera = Camera.main.transform;
        float positionZ = camera.position.z;

        List<Vector2> points = new List<Vector2>();

        Vector2 newPoint = new Vector2(UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude), UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude));
        points.Add(newPoint);
        for (int i = 0; i < shakePointsAmount - 1; i++)
        {
            newPoint.x = newPoint.x > 0f ? UnityEngine.Random.Range(-shakeMagnitude, 0f) : UnityEngine.Random.Range(0f, shakeMagnitude);
            newPoint.y = newPoint.y > 0f ? UnityEngine.Random.Range(-shakeMagnitude, 0f) : UnityEngine.Random.Range(0f, shakeMagnitude);
            points.Add(newPoint);
        }

        Vector3 position;
        Vector2 a;
        Vector2 b;
        for (int i = 0; i < shakePointsAmount + 1; i++)
        {
            a = camera.position;
            b = i < shakePointsAmount ? points[i] : Vector2.zero;

            float journeyLength = Vector2.Distance(a, b);
            //float fractionToMove = (journeyLength * Time.deltaTime) / journeyDuration;
            float fractionToMove = shakeSpeed * Time.deltaTime;
            while ((Vector2)camera.position != b)
            {
                float distanceCovered = Vector2.Distance(a, camera.position);
                float fractionMoved = distanceCovered / journeyLength;

                position = camera.position;
                position = Vector2.Lerp(a, b, fractionMoved + fractionToMove);
                position.z = positionZ;
                camera.position = position;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        End();
    }
}