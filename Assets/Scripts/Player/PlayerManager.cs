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
    public AudioClip doubleJumpSFX = null;
    public AudioClip footstepSFX = null;
    public AnimationReceiver footstepReceiver = null;

    [Header("Abilities")]
    public bool doubleJumpUnlocked;
    public GameObject doubleJumpEffectPrefab = null;
    public bool doubleJumpUsed = false;

    void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        if (footstepSFX != null && footstepReceiver != null)
        {
            footstepReceiver.OnTrigger += PlayFootstepSFX;
        }
    }

    void PlayFootstepSFX()
    {
        GlobalAudio.instance.PlayOneShot(footstepSFX);
    }
}