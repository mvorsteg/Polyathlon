using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public float itemRespawnTime = 3;
    private GameObject child;
    private Collider itemCollider;

    void Start()
    {
        child = transform.GetChild(0).gameObject;
        itemCollider = GetComponent<Collider>();
    }


    // Called when a racer picks up this item
    public virtual void Pickup(Racer racer)
    {
        StartCoroutine(RespawnItem());
    }

    /* when the item is picked up, it should seem to disappear,
       then reappear after itemRespawnTime seconds */
    protected IEnumerator RespawnItem()
    {
        child.SetActive(false);
        itemCollider.enabled = false;
        yield return new WaitForSeconds(itemRespawnTime);
        child.SetActive(true);
        itemCollider.enabled = true;
    }


}
