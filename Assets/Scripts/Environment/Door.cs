using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] AudioClip openSFX = null;
    [SerializeField] [ReadOnlyField] bool opened = false;

    public void Open()
    {
        if (!opened)
        {
            animator.SetTrigger("Open");

            if (openSFX != null)
            {
                GlobalAudio.instance.PlayOneShot(openSFX);
            }

            opened = true;
        }
    }
}