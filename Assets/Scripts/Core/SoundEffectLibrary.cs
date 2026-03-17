using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    public static SoundEffectLibrary Instance { get; private set; }

    [SerializeField]
    private SoundEffectGroup[] soundEffectGroups;

    private Dictionary<string, List<AudioClip>> soundsDictionary;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        soundsDictionary = new();

        foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            soundsDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundsDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundsDictionary[name];

            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }

        return null;
    }
}

[Serializable]
public struct SoundEffectGroup
{
    public string name;
    public List<AudioClip> audioClips;
}
