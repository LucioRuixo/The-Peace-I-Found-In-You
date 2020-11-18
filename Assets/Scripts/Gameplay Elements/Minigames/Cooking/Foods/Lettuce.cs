using UnityEngine;

public class Lettuce : Food
{
    int currentSpriteIndex = -1;

    [SerializeField] Sprite[] additionalSprites;

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

    protected override void TakeCut()
    {
        currentSpriteIndex++;
        spriteRenderer.sprite = additionalSprites[currentSpriteIndex];

        base.TakeCut();
    }
}