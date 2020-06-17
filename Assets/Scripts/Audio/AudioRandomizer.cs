using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips = null;

    int lastClipIndex = 0;

    int GetNewIndex()
    {
        int nextClipIndex = Random.Range(0, clips.Length);
        if (nextClipIndex == lastClipIndex)
        {
            nextClipIndex++;
            if (nextClipIndex == clips.Length)
                nextClipIndex = 0;
        }
        lastClipIndex = nextClipIndex;
        return nextClipIndex;
    }

    public void PlayOneShot()
    {
        GlobalAudio.instance.PlayOneShot(clips[GetNewIndex()]);
    }
}