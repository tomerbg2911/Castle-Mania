using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start()
    {
        Play("Game Tune");
    }

    public Sound getSoundIfEnabled(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError(string.Format("sound {0} was not found. check AudioManager Script", name));
            return null;
        }

        if (s.disable)
        {
            Debug.LogError(string.Format("sound {0} is disabled. check AudioManager Script", name));
            return null;
        }

        return s;
    }

    public void Play(string name)
    {
        Sound s = getSoundIfEnabled(name);
        if(s != null)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = getSoundIfEnabled(name);
        if (s != null && s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

   

}
