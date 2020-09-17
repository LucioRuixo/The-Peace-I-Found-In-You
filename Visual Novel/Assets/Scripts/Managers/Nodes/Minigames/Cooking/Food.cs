using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    bool active = false;
    bool colliding = false;
    bool cut = false;

    public float speed;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    Vector2 screenBounds;
    Vector3 movement;

    SpriteRenderer spriteRenderer;

    static public event Action OnFoodCut;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        height = spriteRenderer.bounds.size.y / 2f;

        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);
        leftScreenLimit = screenBounds.x * -1;
        rightScreenLimit = screenBounds.x;
        lowerScreenLimit = screenBounds.y * -1;
        upperScreenLimit = screenBounds.y;

        movement = Vector3.zero;
        movement.y = speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Knife") colliding = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Knife") colliding = false;
    }

    void Update()
    {
        transform.localPosition += movement * Time.deltaTime;

        if (active)
        {
            if (OffScreen()) Destroy(gameObject);
        }
        else if (!OffScreen()) active = true;

        if (!cut && colliding && Input.GetButtonDown("Left Click"))
        {
            spriteRenderer.color = Color.red;
            cut = true;

            OnFoodCut?.Invoke();
        }
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