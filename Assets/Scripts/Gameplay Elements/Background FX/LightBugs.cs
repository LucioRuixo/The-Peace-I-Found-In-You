using System.Collections.Generic;
using UnityEngine;

public class LightBugs : MonoBehaviour
{
    public enum LightBugsColor
    {
        Yellow,
        Blue
    }

    [SerializeField] Color yellow = Color.yellow;
    [SerializeField] Color blue = Color.blue;

    [SerializeField] Transform defaultYellowBugContainer = null;
    [SerializeField] Transform defaultBlueBugContainer = null;
    [SerializeField] Transform randomBugContainer = null;
    [SerializeField] GameObject bugPrefab = null;

    List<GameObject> activeBugs = new List<GameObject>();

    [Header("Random Generation: ")]
    [SerializeField] int bugAmount = 5;

    [SerializeField] float minSize = 10f;
    [SerializeField] float maxSize = 50f;
    [SerializeField] float minFadeDuration = 1f;
    [SerializeField] float maxFadeDuration = 5f;

    public bool IsPlaying { private set; get; } = false;

    void GenerateRandomly(LightBugsColor bugColor)
    {
        Color color;
        switch (bugColor)
        {
            case LightBugsColor.Yellow:
                color = yellow;
                break;
            case LightBugsColor.Blue:
                color = blue;
                break;
            default:
                color = Color.white;
                break;
        }

        for (int i = 0; i < bugAmount; i++)
        {
            Vector2 position;
            position.x = Random.Range(0f, Screen.width);
            position.y = Random.Range(0f, Screen.height);

            LightBug newBug = Instantiate(bugPrefab, position, Quaternion.identity, randomBugContainer).GetComponent<LightBug>();

            float size = Random.Range(minSize, maxSize);
            float fadeDuration = Random.Range(minFadeDuration, maxFadeDuration);
            newBug.Initialize(size, fadeDuration, color);

            activeBugs.Add(newBug.gameObject);
        }
    }

    public void Play(LightBugsColor bugColor, bool random)
    {
        if (random)
        {
            GenerateRandomly(bugColor);
            return;
        }

        Transform bugContainer;
        Color color;
        switch (bugColor)
        {
            case LightBugsColor.Yellow:
                color = yellow;
                bugContainer = defaultYellowBugContainer;
                break;
            case LightBugsColor.Blue:
                color = blue;
                bugContainer = defaultBlueBugContainer;
                break;
            default:
                color = Color.white;
                bugContainer = null;
                break;
        }
        bugContainer.gameObject.SetActive(true);
        foreach (Transform bug in bugContainer)
        {
            float size = Random.Range(minSize, maxSize);
            float fadeDuration = Random.Range(minFadeDuration, maxFadeDuration);
            LightBug newBug = bug.GetComponent<LightBug>();
            newBug.Initialize(size, fadeDuration, color);

            activeBugs.Add(newBug.gameObject);
        }

        IsPlaying = true;
    }

    public void Stop()
    {
        defaultYellowBugContainer.gameObject.SetActive(false);
        defaultBlueBugContainer.gameObject.SetActive(false);

        foreach (GameObject bug in activeBugs)
        {
            Destroy(bug);
        }
        activeBugs.Clear();

        IsPlaying = false;
    }
}