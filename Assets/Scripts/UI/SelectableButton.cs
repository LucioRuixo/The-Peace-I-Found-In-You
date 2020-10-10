using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float selectedIconSpacing = 20f;

    [SerializeField] RectTransform selectionIcon = null;
    Button button;
    Transform text;

    void Awake()
    {
        button = GetComponent<Button>();
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

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionIcon.gameObject.SetActive(false);
    }
}