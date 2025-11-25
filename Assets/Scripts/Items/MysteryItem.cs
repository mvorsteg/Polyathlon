using UnityEngine;

public class MysteryItem : Item
{
    public Item[] items;
    public float lift = 1f;
    public float liftTime = 3f;
    public AnimationCurve animationCurve;
    public float rotationRate = 30f;

    private float startingY;

    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
        items[Random.Range(0, items.Length)].Pickup(racer);
    }

    protected override void Start()
    {
        base.Start();
        startingY = transform.position.y;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationRate * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, startingY + lift * animationCurve.Evaluate(Time.time % animationCurve.keys[animationCurve.length - 1].time), transform.position.z);
    }

}
