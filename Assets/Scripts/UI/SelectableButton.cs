using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : Button
{
    public float selectedIconSpacing;

    public RectTransform selectionIcon;
    Transform text;

    protected override void Awake()
    {
        base.Awake();

        text = transform.GetChild(0);
    }

    void DisplayIcon()
    {
        selectionIcon.SetParent(text);
        Vector2 position = new Vector2(selectedIconSpacing, 0f);
        selectionIcon.anchoredPosition = position;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        DisplayIcon();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        DisplayIcon();
    }
}