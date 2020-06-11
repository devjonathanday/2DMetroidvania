﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator = null;
    [SerializeField] AudioClip openSFX = null;
    [SerializeField] AudioClip closeSFX = null;
    bool isOpen = false;
    bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            animator.SetBool("StartOpen", true);
        }
    }

    public void Open()
    {
        if (!IsOpen)
        {
            animator.SetTrigger("Open");

            if (openSFX != null)
            {
                GlobalAudio.instance.PlayOneShot(openSFX);
            }

            IsOpen = true;
        }
    }

    public void StartOpen()
    {
        IsOpen = true;
    }

    public void Close()
    {
        animator.SetTrigger("Close");

        if (openSFX != null)
        {
            GlobalAudio.instance.PlayOneShot(closeSFX);
        }

        IsOpen = false;
    }
}