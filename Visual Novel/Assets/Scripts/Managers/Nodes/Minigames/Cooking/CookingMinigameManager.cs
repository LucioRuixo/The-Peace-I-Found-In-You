using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingMinigameManager : MonoBehaviour
{
    public int cuttingTarget;
    public int timer;

    float currentlyCut = 0f;

    public Slider progressBar;
    public TextMeshProUGUI timerText;

    public FoodGenerator foodGenerator;

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
        StartCoroutine(Timer());
    }

    void IncreaseCurrentlyCut()
    {
        currentlyCut++;
        progressBar.value = currentlyCut / cuttingTarget;

        CheckCutAmount();
    }

    void CheckCutAmount()
    {
        if (currentlyCut >= cuttingTarget)
        {
            GameFinished(true);
        }
    }

    void GameFinished(bool playerWon)
    {
        string endText = playerWon ? "ganastes" : "perdistes";
        Debug.Log(endText);

        foodGenerator.generationActive = false;
        StopCoroutine(Timer());
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
        GameFinished(false);
    }
}