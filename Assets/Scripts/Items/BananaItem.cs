using UnityEngine;

public class BananaItem : Item
{
    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
    }

    public override void Use(Racer racer)
    {
        Instantiate(Child, racer.ItemDropPoint, Quaternion.identity);
        racer.EquipItem(null);
    }
}
