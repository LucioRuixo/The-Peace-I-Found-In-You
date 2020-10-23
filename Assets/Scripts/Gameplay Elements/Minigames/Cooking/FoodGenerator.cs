using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    public bool generationActive = true;

    public float initialWaitTime;
    public float waitTime;
    public float impulse;
    public float angleRange;
    public float minX;
    public float maxX;

    public List<GameObject> foodPrefabs = new List<GameObject>();

    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(initialWaitTime);

        while (generationActive)
        {
            Vector2 position = transform.position;
            position.x = Random.Range(minX, maxX);

            float addedAngle = Random.Range(-(angleRange / 2f), angleRange / 2f);
            Quaternion addedRotation = Quaternion.Euler(0f, 0f, addedAngle);
            Quaternion rotation = transform.rotation * addedRotation;

            GameObject prefab = foodPrefabs[Random.Range(0, foodPrefabs.Count)];
            Food newFood = Instantiate(prefab, position, rotation, transform).GetComponent<Food>();
            newFood.SetFall(impulse, position, rotation);

            yield return new WaitForSeconds(waitTime);
        }
    }
}