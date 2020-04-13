using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Animation")]
    [ReadOnlyField] public bool facingRight = true;

    [Header("Audio")]
    public AudioSource jumpSFX;
    public AudioSource landSFX;
}