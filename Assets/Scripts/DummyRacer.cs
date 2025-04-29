using UnityEngine;

public class DummyRacer : Racer
{
    protected override void Start()
    {
        dead = true;
    }

    protected override void FixedUpdate()
    {

    }

    public override void Die(bool emphasizeTorso, Vector3 newMomentum = default(Vector3))
    {
        
    }
}