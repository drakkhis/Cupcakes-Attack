using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class setSliderlvl : MonoBehaviour
{
    public AudioMixer masterMixer2;

    private void Start()
    {
        if (this.name == "sfxSlider")
        {
            Slider slider = this.GetComponent<Slider>();
            slider.value = GetSfxLvl();
        }
        else
        {
            Slider slider = this.GetComponent<Slider>();
            slider.value = GetMusicLvl();
        }
        
    }
    private float GetSfxLvl()
    {
        float result = 0f;
        masterMixer2.GetFloat("sfxVolume", out result);
        return result;
    }

    private float GetMusicLvl()
    {
        float result = 0f;
        masterMixer2.GetFloat("musicVolume", out result);
        return result;
    }
}