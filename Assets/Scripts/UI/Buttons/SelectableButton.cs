using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler
{
    [SerializeField] bool adjustIconToTextLength = false;

    [SerializeField] float selectionIconSpacing = 20f;

    Button button;
    Transform text;

    public RectTransform SelectionIcon { set; get; }
    public bool AdjustIconToTextLength
    {
        set { adjustIconToTextLength = value; }
        get { return adjustIconToTextLength; }
    }

    void Awake()
    {
        button = GetComponent<Button>();
        text = transform.GetChild(0);
    }

    public void DisplayIcon()
    {
        SelectionIcon.gameObject.SetActive(true);

        Transform selectionIconParent = adjustIconToTextLength ? text : transform;
        SelectionIcon.SetParent(selectionIconParent);

        Vector2 position = new Vector2(selectionIconSpacing, 0f);
        SelectionIcon.anchoredPosition = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayIcon();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectionIcon.gameObject.SetActive(false);

        SoundManager.Get().PlaySFX(SoundManager.SFXs.Button);
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