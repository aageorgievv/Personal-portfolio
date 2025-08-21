using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlotHandler : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public Image icon;
    public TMP_Text itemCountText;
    public InventoryManager inventoryManager;

    private void Start()
    {
        if (item != null)
        {
            item = item.Clone();
            icon.sprite = item.itemIcon;
            itemCountText.text = item.itemCount.ToString();
        } else
        {
            icon.gameObject.SetActive(false);
            itemCountText.text = string.Empty;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item == null)
            {
                return;
            }

            MouseManager.instance.PickUpFromStack(this);
            return;
        }

        MouseManager.instance.UpdateHeldItem(this);
    }
}
