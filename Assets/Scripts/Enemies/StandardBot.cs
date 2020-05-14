using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot : Enemy
{
    [SerializeField] GameObject destroyEffect = null;
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
    public override void Death()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}