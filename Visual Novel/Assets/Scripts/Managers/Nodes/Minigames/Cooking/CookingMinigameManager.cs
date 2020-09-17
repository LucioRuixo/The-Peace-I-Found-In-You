using UnityEngine;
using UnityEngine.UI;

public class CookingMinigameManager : MonoBehaviour
{
    public int cuttingTarget;

    float currentlyCut = 0f;

    public Slider progressBar;

    public FoodGenerator foodGenerator;

    void OnEnable()
    {
        Food.OnFoodCut += IncreaseCurrentlyCut;
    }

    void OnDisable()
    {
        Food.OnFoodCut -= IncreaseCurrentlyCut;
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
            Debug.Log("ganastes");
            foodGenerator.generationActive = false;
        }
    }
}