using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler
{
    [SerializeField] float selectedIconSpacing = 20f;

    Button button;
    Transform text;

    public RectTransform SelectionIcon { get; set; }

    void Awake()
    {
        button = GetComponent<Button>();
        text = transform.GetChild(0);
    }

    public void DisplayIcon()
    {
        SelectionIcon.gameObject.SetActive(true);
        SelectionIcon.SetParent(text);

        Vector2 position = new Vector2(selectedIconSpacing, 0f);
        SelectionIcon.anchoredPosition = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayIcon();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectionIcon.gameObject.SetActive(false);

        SoundManager.Get().PlaySFX(SoundManager.SFXs.Fx_ApretaBoton);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SelectionIcon.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        DisplayIcon();

        ((ISelectHandler)button).OnSelect(eventData);
    }

    public void SetSelectionIcon(RectTransform _selectionIcon)
    {
        SelectionIcon = _selectionIcon;
    }
}