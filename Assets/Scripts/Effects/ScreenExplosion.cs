using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenExplosion : MonoBehaviour
{
    [SerializeField] Renderer mainRenderer = null;
    float spawnTime = 0;
    [SerializeField] float growthRate = 0;
    [SerializeField] float growthDuration = 0;
    float currentSize = 0;
    [SerializeField] float fadeOutTime = 0;
    bool fadeStarted = false;
    [SerializeField] AudioClip explosionSFX = null;

    void Awake()
    {
        spawnTime = Time.time;
        currentSize = 0;
        transform.localScale = Vector3.zero;
        GlobalAudio.instance.PlayOneShot(explosionSFX);
    }

    void Update()
    {
        currentSize += Time.deltaTime * growthRate;
        transform.localScale = Vector3.one * currentSize;

        if (!fadeStarted)
        {
            if (Time.time - spawnTime > growthDuration)
            {
                fadeStarted = true;
                StartCoroutine(FadeOut());
            }
        }
    }

    IEnumerator FadeOut()
    {
        mainRenderer.material.color = Color.white;
        for (float t = fadeOutTime; t > 0; t -= Time.deltaTime)
        {
            mainRenderer.material.color = new Color(1, 1, 1, t / fadeOutTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
