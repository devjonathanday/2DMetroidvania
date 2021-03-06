﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] LayerMask specialCollisionLayers = new LayerMask();
    [SerializeField] float lifeTime = 0;
    [ReadOnlyField] public float damage = 0;
    float spawnTimestamp = 0;

    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;
    [ReadOnlyField] public Vector2 velocity = Vector2.zero;
    [SerializeField] ContactFilter2D contactFilter = new ContactFilter2D();
    //Cached array for containing hit results from RayCasts/BoxCasts
    RaycastHit2D[] castResults = new RaycastHit2D[1];

    [Header("Effects")]
    [SerializeField] GameObject destroyEffect = null;

    [Header("Audio")]
    public AudioClip shootSFX = null;
    public AudioClip impactSFX = null;

    Renderer[] selfRenderers = null;
    bool isVisible;

    void Start()
    {
        selfRenderers = GetComponentsInChildren<Renderer>();
    }

    public void Initialize(Vector2 _velocity, float _damage, AudioClip _shootSFX, AudioClip _impactSFX)
    {
        velocity = _velocity;
        spawnTimestamp = Time.time;
        damage = _damage;
        shootSFX = _shootSFX;
        impactSFX = _impactSFX;
    }

    void Update()
    {
        if(Time.time - spawnTimestamp >= lifeTime)
        {
            Destroy(gameObject);
        }

        isVisible = false;

        for (int i = 0; i < selfRenderers.Length; i++)
        {
            if (selfRenderers[i].isVisible)
            {
                isVisible = true;
                break;
            }
        }

        if (!isVisible) Destroy(gameObject);
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

            //If we hit an object that should react to the bullet
            if (((1 << castResults[0].collider.gameObject.layer) & specialCollisionLayers) != 0)
                CheckSpecial(castResults[0]);

            Terminate();
        }
    }

    void CheckSpecial(RaycastHit2D result)
    {
        //Check if we hit a destructible, then attempt to destroy it
        Destructible destructibleHit = result.collider.gameObject.GetComponent<Destructible>();
        if (destructibleHit != null)
        {
            destructibleHit.DestroySelf();
        }

        //Check if we hit a door, then attempt to open it
        Door doorHit = result.collider.gameObject.GetComponent<Door>();
        if (doorHit != null)
        {
            doorHit.Open(0);
        }

        //Check if we hit an enemy, then deal damage to it
        Enemy enemyHit = result.collider.gameObject.GetComponentInParent<Enemy>();
        if(enemyHit != null)
        {
            enemyHit.TakeDamage(damage);
        }
    }

    void Terminate()
    {
        if(destroyEffect != null)
            Instantiate(destroyEffect, rBody.position, Quaternion.identity);

        if(impactSFX != null)
            GlobalAudio.instance.PlayOneShot(impactSFX);

        Destroy(gameObject);
    }
}