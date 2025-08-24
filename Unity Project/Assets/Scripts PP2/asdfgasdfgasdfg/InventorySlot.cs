using UnityEngine;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour
{
    public bool HasItem => Item != null;
    public Item Item { get; private set; }

    [SerializeField]
    protected Image itemIcon;

    protected virtual void Awake()
    {
        ValidationUtility.ValidateReference(itemIcon, nameof(itemIcon));
    }

    public virtual void AddItem(Item item)
    {
        if (!ValidationUtility.ValidateReference(item, nameof(item)))
        {
            return;
        }

        Item = item;
        itemIcon.sprite = item.Sprite;
    }

    public virtual void ClearItem()
    {
        Item = null;
        itemIcon.sprite = null;
    }
}
