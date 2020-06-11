using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Health health = null;
    [SerializeField] float hitEffectDuration = 0;
    Renderer[] selfRenderers = null;
    protected bool isVisible = false;

    public void Start()
    {
        selfRenderers = GetComponentsInChildren<Renderer>();
        SetHitEffectRecursive(0);

        health = GetComponent<Health>();
        health.OnDeathEvent += Death;
    }

    public void Update()
    {
        isVisible = false;

        for (int i = 0; i < selfRenderers.Length; i++)
        {
            if(selfRenderers[i].isVisible)
            {
                isVisible = true;
                break;
            }
        }
    }

    public virtual void TakeDamage(float amount)
    {
        health.TakeDamage(amount);
        StopCoroutine("HitEffect");
        StartCoroutine(HitEffect(hitEffectDuration));
    }

    void SetHitEffectRecursive(float amount)
    {
        for (int i = 0; i < selfRenderers.Length; i++)
        {
            selfRenderers[i].material.SetFloat("_HitEffect", amount);
        }
    }

    IEnumerator HitEffect(float duration)
    {
        SetHitEffectRecursive(1);
        for (float t = duration; t > 0; t -= Time.deltaTime)
        {
            SetHitEffectRecursive(t / duration);
            yield return null;
        }
        SetHitEffectRecursive(0);
    }
    public virtual void Death() { }
}