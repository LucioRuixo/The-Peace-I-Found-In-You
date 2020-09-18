using System.Collections;
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
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    Vector2 screenBounds;

    public GameObject foodPrefab;

    void Awake()
    {
        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);
        leftScreenLimit = screenBounds.x * -1;
        rightScreenLimit = screenBounds.x;
        lowerScreenLimit = screenBounds.y * -1;
        upperScreenLimit = screenBounds.y;
    }

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

            Food newFood = Instantiate(foodPrefab, position, rotation, transform).GetComponent<Food>();
            newFood.SetMovement(impulse, position, rotation);

            yield return new WaitForSeconds(waitTime);
        }
    }
}