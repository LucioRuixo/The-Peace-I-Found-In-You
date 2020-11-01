using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LightBug : MonoBehaviour
{
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

    public void Initialize(float _size, float _fadeDuration, Color _color)
    {
        size = _size;
        fadeDuration = _fadeDuration;
        color = _color;

        if (rect == null) Debug.Log("rect null");
        rect.sizeDelta = new Vector2(size, size);
        image.color = color;
    }

    IEnumerator Glow()
    {
        while (true)
        {
            fXManager.StartAlphaLerp0To1(image, fadeDuration, () => fullyBright = true);
            yield return new WaitUntil(() => fullyBright == true);

            fXManager.StartAlphaLerp1To0(image, fadeDuration, () => fullyBright = false);
            yield return new WaitUntil(() => fullyBright == false);
        }
    }
}