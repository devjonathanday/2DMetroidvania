using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance = null;
    [SerializeField] AudioSource audioSource = null;

    void Awake()
    {
        //Ensure only one GlobalAudio instance exists in the scene
        GlobalAudio[] globalAudios = FindObjectsOfType<GlobalAudio>();
        if(globalAudios.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //Assign self as static GlobalAudio instance
        if(instance == null) instance = this;

        if(audioSource == null)
        {
            Debug.LogError("GlobalAudio instance is missing AudioSource component!");
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}