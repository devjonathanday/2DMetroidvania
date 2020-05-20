using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot : Enemy
{
    [Header("Gameplay")]
    [SerializeField] AnimationReceiver speedReceiver = null;
    [SerializeField] float baseMoveSpeed = 0;
    float currentMoveSpeed = 0;
    float moveSpeedParameter = 0;

    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;
    //Used for physics
    [SerializeField] BoxCollider2D boxCollider = null;
    //Used for ground collision detection
    [SerializeField] ContactFilter2D contactFilter = new ContactFilter2D();
    [SerializeField] float gravity = 0;
    //Cached array for containing hit results from RayCasts/BoxCasts
    RaycastHit2D[] castResults = new RaycastHit2D[3];
    Vector2 velocity = Vector2.zero;
    bool grounded;

    [Header("Visuals")]
    [SerializeField] GameObject destroyEffect = null;
    [Tooltip("X = Intensity, Y = Duration")]
    [SerializeField] Vector2 deathScreenShake = Vector2.zero;

    [Header("Audio")]
    [SerializeField] AudioClip takeDamageSFX = null;

    new void Start()
    {
        base.Start();
        speedReceiver.OnValueChanged += UpdateMoveSpeed;
        //If the player is to the left of the enemy upon scene load
        if(PlayerManager.instance.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //If the player is to the right
        else transform.localScale = Vector3.one;
    }
    void UpdateMoveSpeed()
    {
        if (grounded)
        {
            moveSpeedParameter = speedReceiver.value;
            currentMoveSpeed = moveSpeedParameter * baseMoveSpeed;
        }
    }

    void FixedUpdate()
    {
        ApplyLocomotion();
        ApplyConstraints();

        rBody.position += velocity;
    }
    void ApplyLocomotion()
    {
        velocity.x = currentMoveSpeed * Time.deltaTime * transform.localScale.x;
        velocity.y -= gravity * Time.deltaTime;
    }
    void ApplyConstraints()
    {
        //Ground Collision
        grounded = false;
        if (Physics2D.BoxCast(boxCollider.bounds.center, (boxCollider.bounds.extents * 2), 0, Vector2.down, contactFilter, castResults, Mathf.Abs(velocity.y)) > 0)
        {
            for (int i = 0; i < castResults.Length; i++)
            {
                if (castResults[i] && castResults[i].collider.gameObject != gameObject && castResults[i].point.y < transform.position.y)
                {
                    rBody.position = new Vector2(rBody.position.x, castResults[i].point.y + boxCollider.bounds.extents.y);
                    velocity.y = 0;
                    grounded = true;
                    break;
                }
            }
        }
        //Wall Collision/Turning
        if (Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, contactFilter, castResults, boxCollider.bounds.extents.x + Mathf.Abs(velocity.x)) > 0)
        {
            for (int i = 0; i < castResults.Length; i++)
            {
                if (castResults[i] && castResults[i].collider.gameObject != gameObject)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                    break;
                }
            }
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        if(takeDamageSFX != null)
            GlobalAudio.instance.PlayOneShot(takeDamageSFX);
    }
    public override void Death()
    {
        if (destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity);

        CameraControls.instance.ScreenShake(deathScreenShake.x, deathScreenShake.y);

        Destroy(gameObject);
    }
}