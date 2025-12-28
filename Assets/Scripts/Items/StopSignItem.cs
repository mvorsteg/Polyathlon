using UnityEngine;

public class StopSignItem : Item
{
    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
    }

    public override void Use(Racer racer)
    {
        Vector3 groundDropPoint = racer.ItemDropPoint;
        if (Physics.Raycast(racer.ItemDropPoint, Vector3.down, out RaycastHit hit))
        {
            groundDropPoint = hit.point;
        }

        StopSignObject obj = Instantiate(Child, groundDropPoint, Quaternion.identity).GetComponent<StopSignObject>();
        racer.EquipItem(null);

        obj.Initialize(racer, racer.movementMode == Movement.Mode.Jetpacking || racer.movementMode == Movement.Mode.Gliding);
        
        racer.PlayMiscSound(soundWhenUsed);
        racer.EquipItem(null);
    }
}
