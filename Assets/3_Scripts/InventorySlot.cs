using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Image icon;
    public Skill skill;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public int index;
    public bool isShortcut;
    private Vector2 originPos;

    public void Init(int index)
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        originPos = rectTransform.anchoredPosition;
        this.index = index;
    }

    public void AddSkill(Skill newSkill)
    {
        skill = newSkill;

        icon.sprite = skill.skillIcon;
        Color color = icon.color;
        color.a = 1;
        icon.color = color;
    }

    public void ClearSlot()
    {
        skill = null;

        icon.sprite = null;
        Color color = icon.color;
        color.a = 0;
        icon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDrop");
        if (eventData.pointerDrag != null)
        {
            Inventory.Instance.ChangeSlot(eventData.pointerDrag.GetComponent<InventorySlot>(), this);
        }
    }
}
