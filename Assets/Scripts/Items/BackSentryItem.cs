using UnityEngine;

public class BackSentryItem : Item
{
    private bool used = false;
    public override void Pickup(Racer racer)
    {
        base.Pickup(racer);
    }

    public override void Use(Racer racer)
    {
        //if (!used)
        {
            //used = true;
            BackSentryObject sentryObj = Instantiate(Child, racer.backpackMount).GetComponent<BackSentryObject>();
            //sentryObj.target = RaceManager.GetClosestRacerAheadOfThisOne(racer);
            sentryObj.Initialize(racer);

            racer.EquipItem(null);
        }
    }
}
