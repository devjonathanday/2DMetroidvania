using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Health health = null;
    [SerializeField] float hitEffectDuration = 0;
    [SerializeField] Renderer selfRenderer = null;
    Material selfMaterial = null;

    void Start()
    {
        selfRenderer = GetComponent<Renderer>();
        selfMaterial = Instantiate(selfRenderer.material);
        selfRenderer.material = selfMaterial;
        selfMaterial.SetFloat("_HitEffect", 0);

        health = GetComponent<Health>();
        health.OnDeathEvent += Death;
    }

    public virtual void TakeDamage(float amount)
    {
        health.TakeDamage(amount);
        StopAllCoroutines();
        StartCoroutine(HitEffect(hitEffectDuration));
    }

    IEnumerator HitEffect(float duration)
    {
        for (float t = duration; t > 0; t -= Time.deltaTime)
        {
            selfMaterial.SetFloat("_HitEffect", t / duration);
            yield return null;
        }
        selfMaterial.SetFloat("_HitEffect", 0);
    }
    public virtual void Death() { }
}