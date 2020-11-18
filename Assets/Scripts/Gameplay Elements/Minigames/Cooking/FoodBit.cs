using UnityEngine;

public class FoodBit : MonoBehaviour, IFallingObject
{
    float minY;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float height = spriteRenderer.bounds.size.y;
        minY = Camera.main.ScreenToWorldPoint(Vector2.zero).y - height;
    }

    void Update()
    {
        if (transform.position.y < minY)
        {
            FallOffScreen();
            return;
        }
    }

    public void Initialize(Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetForce(Vector2 force, ForceMode2D mode)
    {
        rigidBody.AddForce(force, mode);
    }

    public void FallOffScreen()
    {
        Destroy(gameObject);
    }
}