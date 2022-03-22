using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public SoundSettings soundSettings;
    public ResolutionSettings resolutionSettings;
    public GameSettings(SoundSettings soundSettings, ResolutionSettings resolutionSettings)
    {
        this.soundSettings = soundSettings;
        this.resolutionSettings = resolutionSettings;
    }

    public GameSettings()
    {
        soundSettings = new SoundSettings();
        resolutionSettings = new ResolutionSettings();
    }
}

[Serializable]
public class SoundSettings
{
    public float musicVolume;
    public float effectsVolume;
    public float masterVolume;
    public SoundSettings(float musicVolume, float effectsVolume, float masterVolume)
    {
        this.musicVolume = musicVolume ;
        this.effectsVolume = effectsVolume ;
        this.masterVolume = masterVolume;
    }

    public SoundSettings()
    {
        musicVolume = 1;
        effectsVolume = 1;
        masterVolume = 1;
    }
}

[Serializable]
public class ResolutionSettings
{
    public int[] resolution = new int[2];
    //public float brightness;
    public void SetResolution(int[] resolution)
    {
        this.resolution = resolution;
        Screen.SetResolution(resolution[0], resolution[1], true);
    }
    public ResolutionSettings(int[] resolution)
    {
        this.resolution = resolution;
        Screen.SetResolution(resolution[0], resolution[1], true);
    }
    public ResolutionSettings()
    {
        resolution = new int[2] { 1920, 1080 };
        //Screen.SetResolution(resolution[0], resolution[1], true);
    }
}


