using UnityEngine;

public class Carrot : Food
{
    [SerializeField] int sliceAmount = 1;

    [SerializeField] float  minSliceImpulse = 1f;
    [SerializeField] float  maxSliceImpulse = 2f;

    [SerializeField] GameObject foodBitPrefab;
    [SerializeField] Sprite[] sliceSprites = null;

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

    void GenerateSlice()
    {
        float rotationZ = Random.Range(1f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, rotationZ);
        GameObject slice = Instantiate(foodBitPrefab, transform.position, rotation, transform.parent);

        Sprite sprite = sliceSprites[Random.Range(0, sliceSprites.Length)];
        slice.GetComponent<SpriteRenderer>().sprite = sprite;

        float impulse = Random.Range(minSliceImpulse, maxSliceImpulse);
        slice.GetComponent<FoodBit>().SetForce(slice.transform.up * impulse, ForceMode2D.Impulse);
    }

    protected override void CutCompletely()
    {
        spriteRenderer.enabled = false;

        for (int i = 0; i < sliceAmount; i++)
        {
            GenerateSlice();
        }

        base.CutCompletely();
    }
}