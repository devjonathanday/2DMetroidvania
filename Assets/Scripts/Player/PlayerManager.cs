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
    [SerializeField] AudioRandomizer footstepRandomizer = null;
    [SerializeField] AnimationReceiver footstepReceiver = null;

    [Header("Abilities")]
    public GameObject doubleJumpEffectPrefab = null;
    public bool doubleJumpUsed = false;

    void Awake()
    {
        if (instance == null) instance = this;
    }
    void Start()
    {
        if (footstepRandomizer != null && footstepReceiver != null)
        {
            footstepReceiver.OnTrigger += PlayFootstepSFX;
        }
    }

    void PlayFootstepSFX()
    {
        footstepRandomizer.PlayOneShot();
    }
}