using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBullet : Bullet
{
    [SerializeField] Transform[] subBullets = null;
    [SerializeField] Bullet subBulletPrefab = null;
    public void Split()
    {
        for (int i = 0; i < subBullets.Length; i++)
        {
            Bullet newBullet = Instantiate(subBulletPrefab, subBullets[i].position, subBullets[i].rotation);
            newBullet.Initialize(velocity, damage);
        }
        Destroy(gameObject);
    }
}