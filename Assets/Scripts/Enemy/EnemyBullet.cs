using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] LayerMask specialCollisionLayers = new LayerMask();
    //[SerializeField] float lifeTime = 0;
    [ReadOnlyField] public float damage = 0;
    //float spawnTimestamp = 0;

    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;
    [ReadOnlyField] public Vector2 velocity = Vector2.zero;
    [SerializeField] ContactFilter2D contactFilter = new ContactFilter2D();
    //Cached array for containing hit results from RayCasts/BoxCasts
    RaycastHit2D[] castResults = new RaycastHit2D[1];

    void CheckCollision()
    {
        if (Physics2D.Raycast(rBody.position, velocity, contactFilter, castResults, velocity.magnitude) > 0)
        {
            rBody.position = castResults[0].point;

            //If we hit an object that should react to the bullet
            if (((1 << castResults[0].collider.gameObject.layer) & specialCollisionLayers) != 0)
                CheckSpecial(castResults[0]);

            Terminate();
        }
    }

    void CheckSpecial(RaycastHit2D result)
    {
        PlayerManager player = result.collider.gameObject.GetComponentInParent<PlayerManager>();
        if (player != null)
        {
            GameManager.instance.playerHealth.TakeDamage(damage);
        }
    }

    void Terminate()
    {
        Destroy(gameObject);
    }
}