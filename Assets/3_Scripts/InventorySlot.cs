using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private InventoryTooltip tooltip;
    public Skill skill;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public int index;
    public bool isShortcut;
    private Vector2 originPos;
    private Transform parent;
    private int siblingIndex;
    private Transform inventoryPanel;
    private bool isInit = false;

    private void OnEnable()
    {
        if (!isInit) return;

        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parent);
        transform.SetSiblingIndex(siblingIndex);
        rectTransform.anchoredPosition = originPos;
        tooltip.gameObject.SetActive(false);
    }

    public void Init(int index, Transform inventoryPanel)
    {
        isInit = true;
        this.inventoryPanel = inventoryPanel;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        originPos = rectTransform.anchoredPosition;
        this.index = index;
        parent = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
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
        if (skill == null) return;

        tooltip.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skill == null) return;

        canvasGroup.blocksRaycasts = false;
        transform.SetParent(inventoryPanel);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parent);
        transform.SetSiblingIndex(siblingIndex);
        rectTransform.anchoredPosition = originPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Inventory.Instance.ChangeSlot(eventData.pointerDrag.GetComponent<InventorySlot>(), this);
        }
        tooltip.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill == null) return;

        tooltip.Init(skill, inventoryPanel);
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (skill == null) return;

        tooltip.gameObject.SetActive(false);
    }
}
