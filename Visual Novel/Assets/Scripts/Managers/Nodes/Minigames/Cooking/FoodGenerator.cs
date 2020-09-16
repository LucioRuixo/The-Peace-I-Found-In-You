using System.Collections;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    bool generationActive = true;

    public float initialWaitTime;
    public float generationWaitTime;

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
            Instantiate(foodPrefab, transform);
            yield return new WaitForSeconds(generationWaitTime);
        }
    }
}