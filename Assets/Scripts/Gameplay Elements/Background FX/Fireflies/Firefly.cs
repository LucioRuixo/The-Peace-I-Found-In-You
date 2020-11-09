using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Firefly : MonoBehaviour
{
    bool shouldGlow = true;
    bool fullyBright = false;

    float size;
    float fadeDuration;

    Color color;

    RectTransform rect;
    Image image;
    FXManager fXManager;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        fXManager = FXManager.Get();
    }

    void Start()
    {
        StartCoroutine(Glow());
    }

    void OnEnable()
    {
        StartCoroutine(WaitToStartGlowing());
    }

    void OnDisable()
    {
        shouldGlow = false;
    }

    public void Initialize(float _size, float _fadeDuration, Color _color)
    {
        size = _size;
        fadeDuration = _fadeDuration;
        color = _color;

        rect.sizeDelta = new Vector2(size, size);
        image.color = color;
    }

    IEnumerator WaitToStartGlowing()
    {
        yield return new WaitUntil(() => shouldGlow == true);
        StartCoroutine(Glow());
    }

    IEnumerator Glow()
    {
        while (shouldGlow)
        {
            fXManager.StartAlphaLerp0To1(image, fadeDuration, () => fullyBright = true);
            yield return new WaitUntil(() => fullyBright == true);

            fXManager.StartAlphaLerp1To0(image, fadeDuration, () => fullyBright = false);
            yield return new WaitUntil(() => fullyBright == false);
        }

        shouldGlow = true;
    }
}