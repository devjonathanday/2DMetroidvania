using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance = null;
    [SerializeField] List<AudioSourceInstance> audioSources = new List<AudioSourceInstance>();
    public int initialSourceCount = 0;

    public class AudioSourceInstance
    {
        public AudioSource component = null;
        public float volume = 1;
        public bool available = true;

        public float clipStartTime = 0;
        public float clipDuration = 0;

        public AudioSourceInstance(AudioSource _component)
        {
            component = _component;
        }

        public void Update()
        {
            if(Time.time - clipStartTime > clipDuration) { available = true; }
        }
    }

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

        //Instantiate AudioSource components and AudioSourceInstance
        for (int i = 0; i < initialSourceCount; i++) { CreateAudioSource(); }
    }

    void Update()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].Update();
        }
    }

    public void PlayOneShot(AudioClip _clip)
    {
        AudioSourceInstance source = FindAvailable();
        source.volume = 1;
        source.clipStartTime = Time.time;
        source.clipDuration = _clip.length;
        source.component.PlayOneShot(_clip);
        source.available = false;
    }

    public void PlayOneShot(AudioClip _clip, float _volume)
    {
        AudioSourceInstance source = FindAvailable();
        source.volume = _volume;
        source.clipStartTime = Time.time;
        source.clipDuration = _clip.length;
        source.component.PlayOneShot(_clip);
        source.available = false;
    }

    AudioSourceInstance FindAvailable()
    {
        //Find the next audio source marked as available
        for (int i = 0; i < audioSources.Count; i++)
        {
            if(audioSources[i].available) return audioSources[i];
        }
        //Create a new audio source if none are available
        return CreateAudioSource();
    }

    AudioSourceInstance CreateAudioSource()
    {
        //Create a new AudioSource instance and add a new container for it to the list
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        AudioSourceInstance newInstance = new AudioSourceInstance(newAudioSource);
        audioSources.Add(newInstance);
        return newInstance;
    }
}