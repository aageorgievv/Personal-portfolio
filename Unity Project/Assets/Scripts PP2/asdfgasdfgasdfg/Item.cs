using UnityEngine;

public class Item
{
    public string Name { get; }
    public Sprite Sprite { get; }
    public int Cost { get; }

    public Item(string name, Sprite sprite, int cost)
    {
        this.Name = name;
        this.Sprite = sprite;
        this.Cost = cost;
    }
}
