using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
    public enum FireflyColor
    {
        Yellow,
        Blue
    }

    [SerializeField] Color yellow = Color.yellow;
    [SerializeField] Color blue = Color.blue;

    [SerializeField] Transform defaultYellowFireflyContainer = null;
    [SerializeField] Transform defaultBlueFireflyContainer = null;
    [SerializeField] Transform randomFireflyContainer = null;
    [SerializeField] GameObject fireflyPrefab = null;

    List<GameObject> randomFireflies = new List<GameObject>();

    [Header("Random Generation: ")]
    [SerializeField] int fireflyAmount = 5;

    [SerializeField] float minSize = 10f;
    [SerializeField] float maxSize = 50f;
    [SerializeField] float minFadeDuration = 1f;
    [SerializeField] float maxFadeDuration = 5f;

    public bool IsPlaying { private set; get; } = false;

    void GenerateRandomly(FireflyColor fireflyColor)
    {
        randomFireflyContainer.gameObject.SetActive(true);

        Color color;
        switch (fireflyColor)
        {
            case FireflyColor.Yellow:
                color = yellow;
                break;
            case FireflyColor.Blue:
                color = blue;
                break;
            default:
                color = Color.white;
                break;
        }

        for (int i = 0; i < fireflyAmount; i++)
        {
            Vector2 position;
            position.x = Random.Range(0f, Screen.width);
            position.y = Random.Range(0f, Screen.height);

            Firefly newFirefly = Instantiate(fireflyPrefab, position, Quaternion.identity, randomFireflyContainer).GetComponent<Firefly>();

            float size = Random.Range(minSize, maxSize);
            float fadeDuration = Random.Range(minFadeDuration, maxFadeDuration);
            newFirefly.Initialize(size, fadeDuration, color);

            randomFireflies.Add(newFirefly.gameObject);
        }
    }

    public void Play(FireflyColor fireflyColor, bool random)
    {
        if (random)
        {
            GenerateRandomly(fireflyColor);
            return;
        }

        Transform fireflyContainer;
        Color color;
        switch (fireflyColor)
        {
            case FireflyColor.Yellow:
                color = yellow;
                fireflyContainer = defaultYellowFireflyContainer;
                break;
            case FireflyColor.Blue:
                color = blue;
                fireflyContainer = defaultBlueFireflyContainer;
                break;
            default:
                color = Color.white;
                fireflyContainer = null;
                break;
        }
        fireflyContainer.gameObject.SetActive(true);
        foreach (Transform firefly in fireflyContainer)
        {
            float size = Random.Range(minSize, maxSize);
            float fadeDuration = Random.Range(minFadeDuration, maxFadeDuration);
            Firefly newFirefly = firefly.GetComponent<Firefly>();
            newFirefly.Initialize(size, fadeDuration, color);
        }

        IsPlaying = true;
    }

    public void Stop()
    {
        defaultYellowFireflyContainer.gameObject.SetActive(false);
        defaultBlueFireflyContainer.gameObject.SetActive(false);
        randomFireflyContainer.gameObject.SetActive(false);

        foreach (GameObject firefly in randomFireflies)
        {
            Destroy(firefly);
        }
        randomFireflies.Clear();

        IsPlaying = false;
    }
}