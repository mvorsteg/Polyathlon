using UnityEngine;

public class MysteryItem : Item
{
    public Item[] items;

    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
        items[Random.Range(0, items.Length)].Pickup(racer);
    }
}
