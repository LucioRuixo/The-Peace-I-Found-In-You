using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Firefly : MonoBehaviour
{
    bool glowing = false;
    bool fullyBright = false;
    bool canStartGlowing = true;

    float size;
    float fadeDuration;

    Color color;

    RectTransform rect;
    Image image;
    FXManager fXManager;

    Queue<IEnumerator> glowingCoroutines = new Queue<IEnumerator>();

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        fXManager = FXManager.Get();
    }

    void OnEnable()
    {
        //StartCoroutine(WaitToStartGlowing());
        //Debug.Log("execute, shouldGlow = " + shouldGlow);
        glowingCoroutines.Enqueue(Glow());
    }

    void Start()
    {
        ExecuteGlowingCoroutines();
    }

    void OnDisable()
    {
        glowing = false;
    }

    void ExecuteGlowingCoroutines()
    {
        if (glowingCoroutines.Count > 0)
            StartCoroutine(glowingCoroutines.Peek());
    }

    void ExecuteNextGlowingCoroutine()
    {
        if (glowingCoroutines.Count == 0) return;

        glowingCoroutines.Dequeue();

        if (glowingCoroutines.Count == 0) return;

        ExecuteGlowingCoroutines();
    }

    void OnGlowingPhaseEnded(bool brightnessIncreased)
    {
        fullyBright = brightnessIncreased;

        //canStartGlowing = true;
        if (!glowing) ExecuteGlowingCoroutines();
    }

    public void Initialize(float _size, float _fadeDuration, Color _color)
    {
        size = _size;
        fadeDuration = _fadeDuration;
        color = _color;

        rect.sizeDelta = new Vector2(size, size);
        image.color = color;
    }

    IEnumerator Glow()
    {
        glowing = true;

        while (true)
        {
            if (gameObject.name == "Firefly (Test)") Debug.Log("glowing");
            //canStartGlowing = false;
            fXManager.StartAlphaLerp0To1(image, fadeDuration, () => OnGlowingPhaseEnded(true));
            yield return new WaitUntil(() => fullyBright == true);

            //canStartGlowing = false;
            fXManager.StartAlphaLerp1To0(image, fadeDuration, () => OnGlowingPhaseEnded(false));
            yield return new WaitUntil(() => fullyBright == false);
        }
    }
}