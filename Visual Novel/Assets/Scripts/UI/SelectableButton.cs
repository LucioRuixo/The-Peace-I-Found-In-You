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

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        selectionIcon.SetParent(text);
        Vector2 position = new Vector2(selectedIconSpacing, 0f);
        selectionIcon.anchoredPosition = position;
    }
}