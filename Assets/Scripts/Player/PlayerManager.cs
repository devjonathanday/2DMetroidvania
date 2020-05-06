using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    [Header("Animation")]
    [ReadOnlyField] public bool facingRight = true;

    [Header("Audio")]
    public AudioClip jumpSFX = null;
    public AudioClip landSFX = null;

    void Awake()
    {
        if(instance == null) instance = this;
    }
}