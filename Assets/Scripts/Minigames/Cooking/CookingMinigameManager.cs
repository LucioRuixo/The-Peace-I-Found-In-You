using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingMinigameManager : MonoBehaviour
{
    public int cuttingTarget;

    public float timer = 60f;
    float currentlyCut = 0f;


    public FoodGenerator foodGenerator;
    public Slider progressBar;
    public Clock clock;

    static public event Action<bool> OnGameEnd;

    void OnEnable()
    {
        Food.OnFoodCut += IncreaseCurrentlyCut;
        Clock.OnTimeUp += EndGame;
    }

    void OnDisable()
    {
        Food.OnFoodCut -= IncreaseCurrentlyCut;
        Clock.OnTimeUp -= EndGame;
    }

    void Start()
    {
        clock.StartTimer(timer);
    }

    void IncreaseCurrentlyCut()
    {
        currentlyCut++;
        progressBar.value = currentlyCut / cuttingTarget;

        if (currentlyCut >= cuttingTarget) EndGame(true);
    }

    void EndGame(bool win)
    {
        foodGenerator.generationActive = false;
        clock.timerActive = false;

        OnGameEnd?.Invoke(win);
    }
}