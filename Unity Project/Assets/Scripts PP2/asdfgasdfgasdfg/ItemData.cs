using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int cost;

    public Item Create()
    {
        return new Item(itemName, itemSprite, cost);
    }
}
