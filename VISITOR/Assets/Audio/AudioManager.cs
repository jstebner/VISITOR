using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup soundMixer;
    [SerializeField]
    private Slider soundSlider;


    private void Start()
    {
        float soundVol = PlayerPrefs.GetFloat("Master", 1);
        soundSlider.value = soundVol;
        OnSoundSliderValueChange(soundVol);
    }

    public void OnSoundSliderValueChange(float value)
    {
        soundMixer.audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat("Master", value);
        PlayerPrefs.Save();
    }

}