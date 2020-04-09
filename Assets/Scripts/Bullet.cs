using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;
    [SerializeField] [ReadOnlyField] Vector2 velocity = Vector2.zero;
    [SerializeField] ContactFilter2D contactFilter = new ContactFilter2D();
    //Cached array for containing hit results from RayCasts/BoxCasts
    RaycastHit2D[] castResults = new RaycastHit2D[1];

    [Header("Effects")]
    [SerializeField] GameObject destroyEffect = null;

    public void Initialize(Vector2 _velocity)
    {
        velocity = _velocity;
    }

    void FixedUpdate()
    {
        CheckCollision();
        ApplyLocomotion();
    }

    void ApplyLocomotion()
    {
        rBody.position += velocity;
    }

    void CheckCollision()
    {
        if(Physics2D.Raycast(rBody.position, velocity, contactFilter, castResults, velocity.magnitude) > 0)
        {
            rBody.position = castResults[0].point;
            Destroy();
        }
    }

    void Destroy()
    {
        if(destroyEffect != null)
            Instantiate(destroyEffect, rBody.position, Quaternion.identity);

        Destroy(gameObject);
    }
}