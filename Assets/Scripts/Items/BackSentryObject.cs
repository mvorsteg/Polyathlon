using System.Collections;
using UnityEngine;

public class BackSentryObject : MonoBehaviour
{
    [SerializeField] private LaserCannon cannon;
    [SerializeField] private int numberOfShots;
    [SerializeField] private float waitTimeWhenFinished = 0.5f;
    private BackpackMount ownerBackpackMount;

    public void Initialize(Racer owner)
    {
        gameObject.SetActive(true);
        cannon.owner = owner;
        ownerBackpackMount = owner.BackpackMount;

        StartCoroutine(CannonCoroutine());
    }

    private IEnumerator CannonCoroutine()
    {
        ownerBackpackMount.Equip(BackpackOptions.BackSentry);

        yield return StartCoroutine(cannon.AimAndShootCoroutine(numberOfShots));
        yield return new WaitForSeconds(waitTimeWhenFinished);

        ownerBackpackMount.Unequip(BackpackOptions.BackSentry);
    }
}