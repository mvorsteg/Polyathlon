using UnityEngine;

public class SpiedieItem : Item
{
    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
    }

    public override void Use(Racer racer)
    {
        racer.SpeedBoost();
    }
}
