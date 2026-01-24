using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Racer racer;

    public void AssignRacer(Racer racer)
    {
        this.racer = racer;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pick up an item
        if (racer != null && other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            item.Pickup(racer);
        }
    }
}
