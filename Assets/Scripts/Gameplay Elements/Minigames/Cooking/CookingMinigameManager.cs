using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingMinigameManager : MonoBehaviour
{
    bool playing = false;

    [SerializeField] int cuttingTarget = 1;

    [SerializeField] float timer = 60f;
    float currentlyCut = 0f;

    [SerializeField] FoodGenerator foodGenerator = null;
    [SerializeField] Slider progressBar = null;
    [SerializeField] Clock clock = null;
    [SerializeField] GameObject knife = null;

    static public event Action<bool> OnGameEnd;

    void OnEnable()
    {
        playing = true;

        Food.OnCut += IncreaseCurrentlyCut;
        Food.OnFallenUnCut += DecreaseCurrentlyCut;
        Clock.OnTimeUp += EndGame;
    }

    void Start()
    {
        foodGenerator.StartGeneration();
        Cursor.visible = false;

        clock.StartTimer(timer);
    }

    void OnDisable()
    {
        Food.OnCut -= IncreaseCurrentlyCut;
        Food.OnFallenUnCut -= DecreaseCurrentlyCut;
        Clock.OnTimeUp -= EndGame;
    }

    void IncreaseCurrentlyCut()
    {
        if (!playing) return;

        currentlyCut++;
        progressBar.value = currentlyCut / cuttingTarget;

        if (currentlyCut >= cuttingTarget) EndGame(true);
    }

    void DecreaseCurrentlyCut()
    {
        if (!playing) return;

        if (currentlyCut > 0)
        {
            currentlyCut--;
            progressBar.value = currentlyCut / cuttingTarget;
        }
    }

    void EndGame(bool win)
    {
        playing = false;

        foodGenerator.generate = false;
        clock.timerActive = false;
        Cursor.visible = true;

        knife.SetActive(false);

        OnGameEnd?.Invoke(win);
    }
}