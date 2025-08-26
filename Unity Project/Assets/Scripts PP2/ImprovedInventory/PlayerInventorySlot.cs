using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventorySlot : InventorySlot
{
    public int Count { get => count; set
        {
            count = value;
            UpdateCount();
        } 
    }
    private int count;

    [SerializeField]
    private TMP_Text itemCountText;

    protected override void Awake()
    {
        base.Awake();
        ValidationUtility.ValidateReference(itemIcon, nameof(itemIcon));
        ValidationUtility.ValidateReference(itemCountText, nameof(itemCountText));
        itemCountText.gameObject.SetActive(false);
    }

    public override void AddItem(Item item)
    {
        if (!ValidationUtility.ValidateReference(item, nameof(item)))
        {
            return;
        }

        base.AddItem(item);
        Count++;
    }

    public void RemoveItem()
    {
        if (Item == null)
        {
            Debug.LogError($"There is no item to remove");
            return;
        }

        Count--;
        if (Count <= 0)
        {
            ClearItem();
        }
    }

    public override void ClearItem()
    {
        base.ClearItem();
        Count = 0;
    }

    public void SwapItemsWithSlot(PlayerInventorySlot other)
    {
        Item otherItem = other.Item;
        int otherCount = other.Count;

        other.ClearItem();

        if (Item != null)
        {
            for (var i = 0; i < Count; i++)
            {
                other.AddItem(Item);
            }
        }

        ClearItem();

        if (otherItem != null)
        {
            for (int i = 0; i < otherCount; i++)
            {
                AddItem(otherItem);
            }
        }
    }

    private void UpdateCount()
    {
        itemCountText.text = Count.ToString();
        itemCountText.gameObject.SetActive(Count > 0);
    }
}
