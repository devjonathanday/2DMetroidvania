using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEffect : MonoBehaviour
{
    [SerializeField] AudioClip SFX = null;

    void Start()
    {
        if(SFX != null)
        {
            GlobalAudio.instance.PlayOneShot(SFX);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}