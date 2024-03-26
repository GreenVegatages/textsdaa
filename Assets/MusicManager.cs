using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource backMusic;
    public AudioClip sound;
    public AudioClip sound2;
    public AudioClip sound3;

    private List<AudioSource> soucSources = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        for (int i = soucSources.Count - 1; i >= 0; i--)
        {
            if (!soucSources[i].isPlaying)
            {
                Destroy(soucSources[i]);
                soucSources.RemoveAt(i);
            }
        }
    }

    public void Play(int i)
    {
        var souce = gameObject.AddComponent<AudioSource>();
        soucSources.Add(souce);
        switch (i)
        {
            case 1:
                souce.clip = sound;
                break;
            case 2:
                souce.clip = sound2;
                break;
            case 3:
                souce.clip = sound3;
                break;
        }

        souce.loop = false;
        souce.volume = 1;
        souce.Play();
    }
}