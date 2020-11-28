using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    public bool generate = true;

    [SerializeField] float initialWaitTime = 1f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float impulse = 1f;
    [SerializeField] float angleRange = 1f;
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 1f;

    [SerializeField] List<GameObject> foodPrefabs = new List<GameObject>();

    public void StartGeneration()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(initialWaitTime);

        while (generate)
        {
            Vector2 position = transform.position;
            position.x = Random.Range(minX, maxX);

            float addedAngle = Random.Range(-(angleRange / 2f), angleRange / 2f);
            Quaternion addedRotation = Quaternion.Euler(0f, 0f, addedAngle);
            Quaternion rotation = transform.rotation * addedRotation;

            GameObject prefab = foodPrefabs[Random.Range(0, foodPrefabs.Count)];
            Food newFood = Instantiate(prefab, position, rotation, transform).GetComponent<Food>();
            newFood.Initialize(position, rotation);
            newFood.SetForce(Vector2.up * impulse, ForceMode2D.Impulse);

            yield return new WaitForSeconds(waitTime);
        }
    }
}