using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponTypes { DEFAULT, PROJECTILE, PHYSICS }

    [Header("Gameplay")]
    public float damage;
    public bool hasLifeTime;
    public float lifeTime;
    float spawnTimeStamp;
    public float spawnOffset;
    //Condition(s) for this weapon to be used, etc. pressing or holding a button.
    [HideInInspector] public bool usageConditions;

    [Header("Audio")]
    public AudioClip spawnSFX;
    public AudioClip terminateSFX;

    [Header("Effects")]
    public GameObject destroyEffect;

    Renderer[] selfRenderers = null;
    bool isVisible;

    public void Start()
    {
        selfRenderers = GetComponentsInChildren<Renderer>();
        if (spawnSFX != null) GlobalAudio.instance.PlayOneShot(spawnSFX);
        spawnTimeStamp = Time.time;
    }

    public void Update()
    {
        if (hasLifeTime && Time.time - spawnTimeStamp >= lifeTime)
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

    public virtual void UpdateUsageConditions() { }

    public virtual void Initialize(params object[] parameters) { }

    public void Terminate()
    {
        if (destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity);
        if (terminateSFX != null) GlobalAudio.instance.PlayOneShot(terminateSFX);
        Destroy(gameObject);
    }
}

public class ProjectileWeapon : Weapon
{
    [Header("Gameplay")]
    [SerializeField] LayerMask specialCollisionLayers = new LayerMask();
    public int doorPower;

    [Header("Physics")]
    [HideInInspector] public Vector2 velocity;
    public float speed;
    public Rigidbody2D rBody;
    public ContactFilter2D contactFilter;
    RaycastHit2D[] castResults = new RaycastHit2D[1];
    
    /// <summary>
    /// Initializes the weapon with variable parameters, based on weapon type.
    /// Vector2 spawnDirection, Quaternion rotation
    /// </summary>
    /// <param name="spawnOffset"></param>
    public override void Initialize(params object[] parameters)
    {
        //Initialize parameters
        Vector3 spawnDirection = (Vector3)parameters[0];

        velocity = spawnDirection.normalized * speed;
    }

    public void FixedUpdate()
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
        if (Physics2D.Raycast(rBody.position, velocity, contactFilter, castResults, velocity.magnitude) > 0)
        {
            transform.position = castResults[0].point;

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
        if (enemyHit != null)
        {
            enemyHit.TakeDamage(damage);
        }
    }
}