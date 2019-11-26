using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using FMOD.Studio;

public class SettingsMenu : MonoBehaviour
{
    //Audio Buses
    Bus masterBus;
    Bus sfxBus;
    Bus musicBus;
    Bus voiceBus;
    float MusicVolume = .5f;
    float SFXVolume = .5f;
    float VoiceVolume = .5f;
    float MasterVolume = 1f;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        voiceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Voice");
        masterBus.setVolume(.5f);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        //Master Volume
        masterBus.setVolume(volume / 50);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}
