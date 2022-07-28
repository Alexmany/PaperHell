using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider SFXSlider;
    public Slider MusicSlider;

    void Start()
    {
        masterMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
        SFXSlider.value = PlayerPrefs.GetFloat("SFX", -25f);

        masterMixer.SetFloat("MUS", PlayerPrefs.GetFloat("MUS"));
        MusicSlider.value = PlayerPrefs.GetFloat("MUS", -25f);
    }

    public void SetSfxLvl(float sLvl)
    {
        masterMixer.SetFloat("SFX", sLvl);
        PlayerPrefs.SetFloat("SFX", sLvl);
    }

    public void SetMusLvl(float sLvl)
    {
        masterMixer.SetFloat("MUS", sLvl);
        PlayerPrefs.SetFloat("MUS", sLvl);
    }
}
