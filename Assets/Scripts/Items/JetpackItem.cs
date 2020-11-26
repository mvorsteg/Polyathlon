using UnityEngine;

public class JetpackItem : Item
{
    public override void Pickup(Racer racer)
    {
        racer.SetMovementMode(Movement.Mode.Jetpacking);
        base.Pickup(racer);
    }
}
