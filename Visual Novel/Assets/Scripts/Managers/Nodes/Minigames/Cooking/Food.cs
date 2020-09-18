using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    bool active = false;
    bool colliding = false;
    bool cut = false;

    public float minY;
    float height;
    //float leftScreenLimit;
    //float rightScreenLimit;
    //float lowerScreenLimit;
    //float upperScreenLimit;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    static public event Action OnFoodCut;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        height = spriteRenderer.bounds.size.y / 2f;
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
        if (transform.position.y < minY)
            Destroy(gameObject);

        if (!cut && colliding && Input.GetButtonDown("Left Click"))
        {
            spriteRenderer.color = Color.red;
            cut = true;

            OnFoodCut?.Invoke();
        }
    }

    //bool OffScreen()
    //{
    //    float x = transform.position.x;
    //    float y = transform.position.y;
    //
    //    return x < leftScreenLimit - height || x > rightScreenLimit + height
    //           ||
    //           y > upperScreenLimit + height|| y < lowerScreenLimit - height;
    //}

    public void SetMovement(float force, Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigidBody.AddForce(transform.up * force, ForceMode2D.Impulse);
    }
}