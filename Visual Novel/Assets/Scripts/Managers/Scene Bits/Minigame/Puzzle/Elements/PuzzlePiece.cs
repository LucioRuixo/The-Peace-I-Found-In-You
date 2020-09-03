using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    public bool OnBoard { set; get; } = false;
    public bool FixedToBoard { get; set; } = false;
    public Vector2Int ID { get; set; }
    public Vector2Int CurrentTile { get; set; }
    public Vector2 GrabPosition { get; set; }

    bool grabbed = false;

    Vector2 mouseOffset;

    Image image;
    RectTransform rect;

    public static event Action<PuzzlePiece, Vector2> OnPieceReleased;

    void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (FixedToBoard) return;

        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //
        //if (!grabbed)
        //{
        //    if (rect.rect.Contains(mousePosition) && Input.GetMouseButtonDown(0))
        //    {
        //        GrabPosition = transform.position;
        //        mouseOffset = GrabPosition - mousePosition;
        //        grabbed = true;
        //    }
        //}
        //else
        //{
        //    transform.position = mousePosition + mouseOffset;
        //
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        //if (OnPieceReleased != null) OnPieceReleased(this, mousePosition);
        //        grabbed = false;
        //    }
        //}
    }

    public IEnumerator Highlight()
    {
        float fadeDuration = 0.2f;
        float currentColorValue = 1f;
        float targetColorValue = 0.5f;
        float totalDistance = currentColorValue - targetColorValue;

        while (currentColorValue > targetColorValue)
        {
            float subtractedValue = totalDistance / (fadeDuration / Time.deltaTime);
            currentColorValue -= subtractedValue;

            Color newColor = image.color;
            newColor.r = newColor.g = newColor.b = currentColorValue;
            image.color = newColor;

            yield return null;
        }

        while (currentColorValue < 1f)
        {
            float addedValue = totalDistance / (fadeDuration / Time.deltaTime);
            currentColorValue += addedValue;

            Color newColor = image.color;
            newColor.r = newColor.g = newColor.b = currentColorValue;
            image.color = newColor;

            yield return null;
        }
    }
}