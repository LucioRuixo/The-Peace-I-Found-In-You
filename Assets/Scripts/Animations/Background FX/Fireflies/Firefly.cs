using UnityEngine;
using UnityEngine.UI;

public class Firefly : MonoBehaviour
{
    bool glowing = false;

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

    void OnEnable()
    {
        if (!glowing)
        {
            StartIncreasingAlpha();
            glowing = true;
        }
    }

    void StartIncreasingAlpha()
    {
        if (gameObject.activeInHierarchy)
            fXManager.StartAlphaLerp0To1(image, fadeDuration, StartDecreasingAlpha);
        else glowing = false;
    }

    void StartDecreasingAlpha()
    {
        if (gameObject.activeInHierarchy)
            fXManager.StartAlphaLerp1To0(image, fadeDuration, StartIncreasingAlpha);
        else glowing = false;
    }

    public void Initialize(float _size, float _fadeDuration, Color _color)
    {
        size = _size;
        fadeDuration = _fadeDuration;
        color = _color;

        rect.sizeDelta = new Vector2(size, size);
        image.color = color;
    }
}