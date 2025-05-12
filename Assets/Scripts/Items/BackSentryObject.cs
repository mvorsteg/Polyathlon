using System.Collections;
using UnityEngine;

public class BackSentryObject : MonoBehaviour
{
    [SerializeField] private LaserCannon cannon;
    [SerializeField] private int numberOfShots;
    [SerializeField] private float waitTimeWhenFinished = 0.5f;

    public void Initialize(Racer owner)
    {
        cannon.owner = owner;
        StartCoroutine(CannonCoroutine());
    }

    private IEnumerator CannonCoroutine()
    {
        yield return StartCoroutine(cannon.AimAndShootCoroutine(numberOfShots));
        yield return new WaitForSeconds(waitTimeWhenFinished);

        Destroy(this.gameObject);
    }
}