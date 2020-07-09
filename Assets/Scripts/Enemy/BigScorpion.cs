using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigScorpion : Enemy
{
    [SerializeField] enum AIState { Walking, Liftoff, Flying }
    [SerializeField] [ReadOnlyField] AIState currentState = AIState.Walking;

    [Header("Physics")]
    [SerializeField] Rigidbody2D rBody = null;

    [Header("AI")]
    [SerializeField] float walkSpeed = 0;
    [Tooltip("If we are walking, how often should we change direction? (Greater = Less Likely)")]
    [SerializeField] int reverseChance = 0;
    [SerializeField] float reverseIncrement = 0;
    float reverseTimestamp = 0;
    bool walkingLeft = true;
    [SerializeField] Vector2 walkBoundaries = Vector2.zero;
    [SerializeField] Vector3 flightOrigin = Vector3.zero;
    [SerializeField] Vector2 flightRange = Vector2.zero;
    [SerializeField] Vector2 flightSpeed = Vector2.zero;
    Vector2 flightTarget = Vector2.zero;
    [SerializeField] float liftoffDuration = 0;
    [SerializeField] AnimationCurve liftoffCurve = null;

    [Header("Visuals")]
    [SerializeField] Animator animator = null;
    [SerializeField] GameObject explosionPrefab = null;
    [SerializeField] GameObject screenExplosionPrefab = null;
    [SerializeField] GameObject sprite = null;
    [Tooltip("X = Intensity, Y = Duration")]
    [SerializeField] Vector2 deathScreenShake = Vector2.zero;
    [SerializeField] int deathExplosionCount = 0;
    [SerializeField] float deathExplosionIncrement = 0;
    [SerializeField] float deathExplosionOffset = 0;

    [Header("Audio")]
    [SerializeField] AudioClip takeDamageSFX = null;
    [SerializeField] AudioClip liftoffSFX = null;

    new void Start()
    {
        base.Start();
        reverseTimestamp = Time.time;
    }

    new void Update()
    {
        base.Update();
        if(health.dead)
        {
            sprite.SetActive(!sprite.activeSelf);
        }
    }

    void FixedUpdate()
    {
        if (!health.dead)
        {
            switch (currentState)
            {
                case AIState.Walking:
                    {
                        Walk(walkingLeft);
                        CheckReverse();
                    }
                    break;
                case AIState.Liftoff:
                    {
                        flightTarget = new Vector3(flightOrigin.x + (Mathf.Sin(Time.time * flightSpeed.x) * flightRange.x),
                                                   flightOrigin.y + (Mathf.Sin(Time.time * flightSpeed.y) * flightRange.y), 0);
                    }
                    break;
                default:
                case AIState.Flying:
                    {
                        flightTarget = new Vector3(flightOrigin.x + (Mathf.Sin(Time.time * flightSpeed.x) * flightRange.x),
                                                   flightOrigin.y + (Mathf.Sin(Time.time * flightSpeed.y) * flightRange.y), 0);
                        rBody.position = flightTarget;
                        if (rBody.position.x < PlayerManager.instance.transform.position.x)
                            sprite.transform.localScale = new Vector3(-1, 1, 1);
                        else sprite.transform.localScale = new Vector3(1, 1, 1);
                    }
                    break;
            }
        }
    }

    #region AI Behaviors

    void Walk(bool left)
    {
        rBody.position += Vector2.right * (left ? -1 : 1) * walkSpeed * Time.deltaTime;
        animator.SetTrigger(left ? "WalkForward" : "WalkBackward");
    }

    void CheckReverse()
    {
        if (Time.time - reverseTimestamp > reverseIncrement)
        {
            if (rBody.position.x < walkBoundaries.x)
            {
                walkingLeft = false;
            }
            else if (rBody.position.x > walkBoundaries.y)
            {
                walkingLeft = true;
            }
            else if (Random.Range(1, reverseChance + 1) == 1)
            {
                walkingLeft = !walkingLeft;
            }

            reverseTimestamp = Time.time;
        }
    }

    IEnumerator LiftoffSequence()
    {
        Vector2 initialPosition = transform.position;
        flightTarget = new Vector3(flightOrigin.x + (Mathf.Sin(Time.time * flightSpeed.x) * flightRange.x),
                                   flightOrigin.y + (Mathf.Sin(Time.time * flightSpeed.y) * flightRange.y), 0);
        for (float t = 0; t < liftoffDuration; t += Time.deltaTime)
        {
            rBody.position = Vector2.Lerp(initialPosition, flightTarget, liftoffCurve.Evaluate(t / liftoffDuration));
            yield return null;
        }
        currentState = AIState.Flying;
    }

    #endregion

    public override void TakeDamage(float amount)
    {
        if (!health.dead)
        {
            base.TakeDamage(amount);

            if (takeDamageSFX != null)
                GlobalAudio.instance.PlayOneShot(takeDamageSFX);

            if (currentState == AIState.Walking && health.CurrentHealth <= health.maxHealth / 2)
            {
                StartCoroutine(LiftoffSequence());
                animator.SetTrigger("Liftoff");
                currentState = AIState.Liftoff;
                if (liftoffSFX != null)
                    GlobalAudio.instance.PlayOneShot(liftoffSFX);
            }
        }
    }

    public override void Death()
    {
        CameraControls.instance.ScreenShake(deathScreenShake.x, deathScreenShake.y);
        for (int i = 0; i < deathExplosionCount; i++)
        {
            StartCoroutine(TimedExplosion(deathExplosionIncrement * i, explosionPrefab, false, true));
        }
        StartCoroutine(TimedExplosion(deathExplosionIncrement * deathExplosionCount, screenExplosionPrefab, true, false));
    }

    IEnumerator TimedExplosion(float delay, GameObject explosion, bool final, bool randomize)
    {
        for (float t = 0; t < delay; t += Time.deltaTime)
        {
            yield return null;
        }
        Vector3 randomOffset = randomize ? new Vector3(Random.Range(-deathExplosionOffset, deathExplosionOffset), Random.Range(-deathExplosionOffset, deathExplosionOffset)) : Vector3.zero;
        if (explosion != null) Instantiate(explosion, transform.position + randomOffset, Quaternion.identity);
        if (final) Destroy(gameObject);
    }
}