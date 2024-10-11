using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    public AudioMixer am;
    public bool isFullScreen;
    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
    public void AudioVolume(float sliderValue)
    {
        am.SetFloat("MainVolume", sliderValue);
    }
}
