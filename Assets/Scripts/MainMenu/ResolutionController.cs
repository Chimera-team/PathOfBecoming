using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    private GameSettings gameSettings;
    [SerializeField] Dropdown resolutionDropdown;
    private void Awake()
    {
        gameSettings = SaveSyatem.gameSettings;
        int[] res = gameSettings.resolutionSettings.resolution;
        if(res[0]/res[1]!=16/9)
        {
            res[0] = 1920;
            res[1] = 1080;
        }
        Screen.SetResolution(res[0], res[1], true);
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
        widthNheight[1] = int.Parse(res.Substring(position +1));
        return widthNheight;
    }
}
