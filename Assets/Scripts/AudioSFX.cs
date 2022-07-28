using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    public AudioClip stamp_sound;
    public AudioClip grab_paper_sound;
    public AudioClip spawn_paper_sound;
    public AudioClip collect_paper_sound;
    public AudioClip button_sound;
    public AudioClip ready_paper_sound;
    public AudioClip buy_sound;
    public AudioClip error_sound;
    public AudioClip damage_sound;
    public AudioSource sfxSource;
    public AudioSource pencilSound;

    public static AudioSFX instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySingle(AudioClip clip, float v)
    {
        sfxSource.PlayOneShot(clip, v);
    }
}
