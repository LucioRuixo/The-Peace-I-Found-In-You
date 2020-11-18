using System;
using UnityEngine;

public class Food : MonoBehaviour, IFallingObject
{
    bool colliding = false;
    bool cut = false;

    [SerializeField] int necessaryCuts = 1;
    int cuts = 0;

    float minY;

    Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;

    static public event Action OnCut;
    static public event Action OnFallenUnCut;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        float height = spriteRenderer.bounds.size.y;
        minY = Camera.main.ScreenToWorldPoint(Vector2.zero).y - height;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Knife") colliding = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Knife") colliding = false;
    }

    protected virtual void Update()
    {
        if (transform.position.y < minY)
        {
            FallOffScreen();
            return;
        }

        if (!cut && colliding && Input.GetButtonDown("Left Click"))
        {
            TakeCut();

            if (cuts >= necessaryCuts) CutCompletely();
        }
    }

    protected virtual void TakeCut()
    {
        cuts++;
    }

    protected virtual void CutCompletely()
    {
        cut = true;

        OnCut?.Invoke();
    }

    public void Initialize(Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetForce(Vector2 force, ForceMode2D mode = 0)
    {
        rigidBody.AddForce(force, mode);
    }

    public void FallOffScreen()
    {
        if (!cut) OnFallenUnCut?.Invoke();

        Destroy(gameObject);
    }
}