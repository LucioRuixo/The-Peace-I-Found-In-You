using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool OnBoard { set; get; } = false;
    public bool FixedToBoard { get; set; } = false;
    public Vector2Int ID { get; set; }
    public Vector2Int CurrentTile { get; set; }
    public Vector2 GrabPosition { get; set; }

    Vector2 pointerOffset;

    Image image;
    public RectTransform rect;

    public static event Action<Transform> OnDragBeginning;
    public static event Action<PuzzlePiece, Vector2> OnDragEnd;

    void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (FixedToBoard) return;

        pointerOffset = (Vector2)transform.position - eventData.position;
        OnDragBeginning?.Invoke(transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (FixedToBoard) return;

        transform.position = eventData.position + pointerOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (FixedToBoard) return;

        OnDragEnd?.Invoke(this, eventData.position);
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