using System;
using System.Collections;
using TMPro;
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

    static public event Action OnGameEnd;

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

    void EndGame(bool playerWon)
    {
        string endText = playerWon ? "ganastes" : "perdistes";
        Debug.Log(endText);

        foodGenerator.generationActive = false;

        OnGameEnd?.Invoke();
    }
}