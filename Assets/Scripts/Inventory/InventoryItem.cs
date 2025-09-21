using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public InventoryItemSO item;
    public TextMeshProUGUI amoutText;
    public int amount = 0;
    public Image image;
    public Transform parentAfterDrag;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;
    private int clickCount;

    public void InitItem(InventoryItemSO newItem)
    {
        item = newItem;
        image.sprite = newItem.icon;
        RefreshAmount();
    }

    void Start()
    {
        InitItem(item);
    }

    public void RefreshAmount()
    {
        amoutText.text = amount.ToString();
        amoutText.gameObject.SetActive(amount > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.time;

        // Kiểm tra double click
        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            OnDoubleClick();
            clickCount = 0; // Reset sau double click
        }
        else
        {
            clickCount = 1;
        }

        lastClickTime = currentTime;
    }

    private void OnDoubleClick()
    {
        Debug.Log($"Double-click detected on {gameObject.name}");

        // Kiểm tra null và gọi phương thức sử dụng item
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.UsingItem(this);
        }
        else
        {
            Debug.LogWarning("InventoryManager instance is null!");
        }
    }
}