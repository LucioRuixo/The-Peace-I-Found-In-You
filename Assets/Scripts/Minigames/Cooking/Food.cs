using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    bool colliding = false;
    bool cut = false;

    public int necessaryCuts;
    int cuts = 0;

    float height;
    float minY;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    static public event Action OnFoodCut;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        height = spriteRenderer.bounds.size.y;
        minY = Camera.main.ScreenToWorldPoint(Vector2.zero).y - height;
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
            cuts++;
            if (cuts >= necessaryCuts) Cut();
        }
    }

    void Cut()
    {
        spriteRenderer.color = Color.black;
        cut = true;

        OnFoodCut?.Invoke();
    }

    public void SetFall(float force, Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigidBody.AddForce(transform.up * force, ForceMode2D.Impulse);
    }
}