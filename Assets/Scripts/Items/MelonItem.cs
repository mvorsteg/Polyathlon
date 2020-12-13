using UnityEngine;

public class MelonItem : Item
{
    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
    }

    public override void Use(Racer racer)
    {
        racer.ThrowItem();
    }
}
