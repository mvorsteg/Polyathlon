using UnityEngine;

public class GliderItem : Item
{
    public override void Pickup(Racer racer)
    {
        racer.SetMovementMode(Movement.Mode.Gliding);
        base.Pickup(racer);
    }
}
