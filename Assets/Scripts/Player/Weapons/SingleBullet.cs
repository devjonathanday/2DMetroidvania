using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet : ProjectileWeapon
{
    new public void Update()
    {
        base.Update();
        UpdateUsageConditions();
    }

    public override void UpdateUsageConditions()
    {
        usageConditions = InputHandler.instance.player.GetButtonDown("Shoot");
    }
}
