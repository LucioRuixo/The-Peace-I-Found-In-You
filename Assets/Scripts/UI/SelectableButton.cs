using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] float selectedIconSpacing = 20f;

    [SerializeField] RectTransform selectionIcon = null;
    Transform text;

    void Awake()
    {
        text = transform.GetChild(0);
    }

    void DisplayIcon()
    {
        selectionIcon.gameObject.SetActive(true);
        selectionIcon.SetParent(text);

        Vector2 position = new Vector2(selectedIconSpacing, 0f);
        selectionIcon.anchoredPosition = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayIcon();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        selectionIcon.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionIcon.gameObject.SetActive(false);
    }
}