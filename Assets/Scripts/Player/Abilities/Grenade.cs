using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float explosionTime = 0;
    [SerializeField] GameObject explosionEffect = null;
    [SerializeField] AudioClip explosionSFX = null;
    [Tooltip("X = Intensity, Y = Duration")]
    [SerializeField] Vector2 screenShake = Vector2.zero;
    [SerializeField] LayerMask explodeLayers = new LayerMask();
    [SerializeField] Animator animator = null;
    [SerializeField] Rigidbody2D rBody = null;
    bool explosionQueued = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & explodeLayers) != 0)
        {
            rBody.isKinematic = true;
            rBody.velocity = Vector2.zero;
            animator.SetTrigger("Explode");

            Door doorHit = collision.gameObject.GetComponent<Door>();
            if(doorHit != null)
            {
                doorHit.Open(1);
            }
        }
        else if (!explosionQueued)
        {
            explosionQueued = true;
            StartCoroutine(TimedExplosion());
        }
    }

    IEnumerator TimedExplosion()
    {
        for (float t = 0; t < explosionTime; t += Time.deltaTime)
        {
            yield return null;
        }
        rBody.isKinematic = true;
        rBody.velocity = Vector2.zero;
        animator.SetTrigger("Explode");
    }

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        if (explosionSFX != null) GlobalAudio.instance.PlayOneShot(explosionSFX);
        CameraControls.instance.ScreenShake(screenShake.x, screenShake.y);
        Destroy(gameObject);
    }
}