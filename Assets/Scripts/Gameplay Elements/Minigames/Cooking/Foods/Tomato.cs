using UnityEngine;

public class Tomato : Food
{
    [SerializeField] float cutHalfImpulse = 1f;

    [SerializeField] Sprite leftHalfSprite;
    [SerializeField] Sprite rightHalfSprite;
    [SerializeField] GameObject foodBitPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    protected override void Update()
    {
        base.Update();
    }

    void GenerateHalf(Vector2 forceDirection)
    {
        Sprite sprite = forceDirection == Vector2.left ? leftHalfSprite : rightHalfSprite;

        GameObject half = Instantiate(foodBitPrefab, transform.position, Quaternion.identity, transform.parent);
        half.GetComponent<SpriteRenderer>().sprite = sprite;
        half.GetComponent<FoodBit>().SetForce(forceDirection * cutHalfImpulse, ForceMode2D.Impulse);
    }

    protected override void CutCompletely()
    {
        spriteRenderer.enabled = false;

        GenerateHalf(Vector2.left);
        GenerateHalf(Vector2.right);

        base.CutCompletely();
    }
}