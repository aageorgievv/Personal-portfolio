using TMPro;
using UnityEngine;

public class ShopInventorySlot : InventorySlot
{
    [SerializeField]
    private TMP_Text itemName;
    [SerializeField]
    private TMP_Text itemCost;

    protected override void Awake()
    {
        base.Awake();
        ValidationUtility.ValidateReference(itemName, nameof(itemName));
        ValidationUtility.ValidateReference(itemCost, nameof(itemCost));


    }

    public override void AddItem(Item item)
    {
        base.AddItem(item);
        itemName.text = item.Name;
        itemCost.text = $"{item.Cost} coins";
    }
}
