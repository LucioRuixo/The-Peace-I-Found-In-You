using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingMinigameManager : MonoBehaviour
{
    public int cuttingTarget;
    public int timer;

    float currentlyCut = 0f;

    IEnumerator timerCorroutine;

    public Slider progressBar;
    public TextMeshProUGUI timerText;

    public FoodGenerator foodGenerator;

    static public event Action OnGameEnd;

    void OnEnable()
    {
        Food.OnFoodCut += IncreaseCurrentlyCut;
    }

    void OnDisable()
    {
        Food.OnFoodCut -= IncreaseCurrentlyCut;
    }

    void Start()
    {
        timerCorroutine = Timer();
        StartCoroutine(timerCorroutine);
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
        StopCoroutine(timerCorroutine);

        OnGameEnd?.Invoke();
    }

    IEnumerator Timer()
    {
        while (timer > 0)
        {
            timerText.text = timer.ToString();
            timer--;

            yield return new WaitForSeconds(1f);
        }

        timerText.text = timer.ToString();
        EndGame(false);
    }
}