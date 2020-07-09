using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageExplosion : MonoBehaviour
{
    List<GameObject> hitObjects = null;
    [SerializeField] float damage = 0;
    [SerializeField] LayerMask enemyLayers = new LayerMask();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & enemyLayers) != 0)
        {
            if (!hitObjects.Contains(collider.gameObject))
            {
                hitObjects.Add(collider.gameObject);
                collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
}