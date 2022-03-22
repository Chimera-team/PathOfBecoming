using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlidersController : MonoBehaviour
{
    // [SerializeField] private Slider masterVolSlider;
    public AudioSource musicSound;
    private GameSettings gameSettings;
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Slider masterVolSlider;
    [SerializeField] Slider musicVolSlider;
    [SerializeField] Slider effectsVolSlider;
    //private GameSettings gameSettings;
    private void Start()
    {
        try
        {
            gameSettings = SaveSyatem.gameSettings;
        }
        catch
        {
            gameSettings = new GameSettings();
            SaveSyatem.gameSettings = gameSettings;
        }
        int[] res = gameSettings.resolutionSettings.resolution;
        if (res[0] / res[1] != 16 / 9)
        {
            res[0] = 1920;
            res[1] = 1080;
        }
        masterVolSlider.value = gameSettings.soundSettings.masterVolume;
        musicVolSlider.value = gameSettings.soundSettings.musicVolume;
        effectsVolSlider.value = gameSettings.soundSettings.effectsVolume;
        
        Screen.SetResolution(res[0], res[1], true);
        
    }
    public void OnMasterSliderValueChange(float val)
    {
        gameSettings.soundSettings.masterVolume = val;
        musicSound.volume = gameSettings.soundSettings.musicVolume * val;
    }
    public void OnMusicSliderValueChange(float val)
    {
        gameSettings.soundSettings.musicVolume = val;
    }
    public void OnSoundSliderValueChange(float val)
    {
        gameSettings.soundSettings.effectsVolume = val;
    }
    public void SaveAndExit()
    {
        SaveSyatem.gameSettings = gameSettings;

    }
    public void Change()
    {
        int[] widthHeight = GetResolutions(resolutionDropdown.value);
        // Screen.SetResolution(widthHeight[0],widthHeight[1], true);
        gameSettings.resolutionSettings.SetResolution(widthHeight);
    }
    public void FullScreen(bool isFullscreen)
    {
        Screen.fullScreen = !isFullscreen;
    }
    private int[] GetResolutions(int option)
    {
        string res = resolutionDropdown.options[option].text;
        int[] widthNheight = new int[2];
        int position = res.IndexOf("x");
        widthNheight[0] = int.Parse(res.Substring(0, position));
        widthNheight[1] = int.Parse(res.Substring(position + 1));
        return widthNheight;
    }
}
