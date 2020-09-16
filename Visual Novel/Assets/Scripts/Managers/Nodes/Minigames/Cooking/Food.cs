using UnityEngine;

public class Food : MonoBehaviour
{
    bool active = false;

    public float speed;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    Vector2 screenBounds;
    Vector3 movement;

    void Awake()
    {
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;

        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);
        leftScreenLimit = screenBounds.x * -1;
        rightScreenLimit = screenBounds.x;
        lowerScreenLimit = screenBounds.y * -1;
        upperScreenLimit = screenBounds.y;

        movement = Vector3.zero;
        movement.y = speed;
    }

    void Update()
    {
        transform.localPosition += movement * Time.deltaTime;

        if (active)
        {
            if (OffScreen()) Destroy(gameObject);
        }
        else if (!OffScreen()) active = true;
    }

    bool OffScreen()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        return x < leftScreenLimit - height || x > rightScreenLimit + height
               ||
               y > upperScreenLimit + height|| y < lowerScreenLimit - height;
    }
}