using UnityEngine;

public class SpiedieItem : Item
{
    public override void Pickup(Racer racer)
    {
        StartCoroutine(racer.SpeedBoost(2f, 5f));
        base.Pickup(racer);
    }
}
