using System.Collections;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    public bool generationActive = true;

    public float initialWaitTime;
    public float generationWaitTime;
    public float foodSpeed;

    public GameObject foodPrefab;

    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(initialWaitTime);

        while (generationActive)
        {
            Food newFood = Instantiate(foodPrefab, transform).GetComponent<Food>();
            newFood.SetSpeed(foodSpeed);

            yield return new WaitForSeconds(generationWaitTime);
        }
    }
}