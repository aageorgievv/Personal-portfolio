using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour, IManager
{
    [SerializeField]
    private PlayerInventory playerInventory;
    

    [SerializeField]
    private Image heldItemSprite;

    private PlayerInventorySlot selectedSlot;

    private void Awake()
    {
        ValidationUtility.ValidateReference(playerInventory, nameof(playerInventory));
        ValidationUtility.ValidateReference(heldItemSprite, nameof(heldItemSprite));
        heldItemSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateMouseLeft();
        UpdateMouseRight();
        UpdateItemIcon();
    }

    private void UpdateMouseLeft()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            List<RaycastResult> results = RaycastOnMouse();

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out PlayerInventorySlot playerSlot))
                {
                    HandleLeftClickedPlayerSlot(playerSlot);
                    continue;
                }

            }
        }
    }

    private void UpdateMouseRight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            List<RaycastResult> results = RaycastOnMouse();
            foreach(RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out PlayerInventorySlot playerSlot))
                {
                    HandleRightClickedPlayerSlot(playerSlot);
                    continue;
                }

                if (result.gameObject.TryGetComponent(out ShopInventorySlot shopSlot))
                {
                    HandleClickedShopSlot(shopSlot);
                }
            }

            //ClearHeldItem();
        }
    }

    private void UpdateItemIcon()
    {
        Vector3 mousePosition = Input.mousePosition;
     
        if (selectedSlot != null)
        {
            heldItemSprite.rectTransform.position = mousePosition;
        }
    }

    private void ClearHeldItem()
    {
        heldItemSprite.gameObject.SetActive(false);
        selectedSlot = null;
    }

    private List<RaycastResult> RaycastOnMouse()
    {
        PointerEventData m_PointerEventData = new PointerEventData(EventSystem.current);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(m_PointerEventData, results);
        return results;
    }

    private void HandleLeftClickedPlayerSlot(PlayerInventorySlot slot)
    {
        if (selectedSlot != null)
        {
            selectedSlot.SwapItemsWithSlot(slot);
            ClearHeldItem();
            return;
        }

        selectedSlot = slot;
        heldItemSprite.sprite = slot.Item.Sprite;
        heldItemSprite.gameObject.SetActive(true);
    }

    private void HandleRightClickedPlayerSlot(PlayerInventorySlot slot)
    {
        // detect if shop is open, and sell the item, etc
    }

    private void HandleClickedShopSlot(ShopInventorySlot slot)
    {
        if (!slot.HasItem)
        {
            return;
        }

        // purchase the item
        playerInventory.AddItem(slot.Item);
    }


    public void UpdateHeldItem(UISlotHandler currentSlot)
    {
        //ItemData currentActiveItem = currentSlot.item;

        //if (currentlyHeldItem != null && currentActiveItem != null && currentlyHeldItem.itemName == currentActiveItem.itemName)
        //{
        //    playerInventory.StackInInventory(currentSlot, currentlyHeldItem);
        //    currentlyHeldItem = null;
        //    return;
        //}

        //if(currentSlot.item != null)
        //{
        //    playerInventory.ClearItemSlot(currentSlot);
        //}

        //if(currentlyHeldItem != null)
        //{
        //    playerInventory.PlaceInInventory(currentSlot,currentlyHeldItem);
        //}

        //currentlyHeldItem = currentActiveItem;
    }

    public void PickUpFromStack(UISlotHandler currentSlot)
    {
        //if (ValidationUtility.ValidateReference(currentSlot.item, nameof(currentSlot.item))) {
        //    return;
        //}

        //if(currentlyHeldItem != null && currentlyHeldItem.itemName != currentSlot.item.itemName)
        //{
        //    return;
        //}

        //if(currentlyHeldItem == null)
        //{
        //    currentlyHeldItem = currentSlot.item.Clone();
        //    currentlyHeldItem.itemCount = 0;
        //}

        //currentlyHeldItem.itemCount++;
        //currentSlot.item.itemCount--;
        //currentSlot.itemCountText.text = currentSlot.item.itemCount.ToString();

        //if (currentSlot.item.itemCount <= 0)
        //{
        //    playerInventory.ClearItemSlot(currentSlot);
        //}
    }
}
