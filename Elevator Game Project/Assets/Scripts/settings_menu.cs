using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class settings_menu : MonoBehaviour
{
    public AudioMixer mainMixer;

    public Dropdown resolutionDropdown;

    [SerializeField] Slider volumeSlider;

    Resolution[] resolutions;

    public Dropdown regionDropdown;

    string[] regions = { "asia", "au", "cae", "eu", "in", "jp", "ru", "rue", "za", "sa", "kr", "tr", "us", "usw" };

    void Start()
    {
        resolutions = new Resolution[3];

        resolutions[0].width = 1280;
        resolutions[0].height = 720;

        resolutions[1].width = 1920;
        resolutions[1].height = 1080;

        resolutions[2].width = 3840;
        resolutions[2].height = 2160;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
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

        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        AudioListener.volume = Mathf.Clamp01(volumeSlider.value);
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        //mainMixer.SetFloat("volume", volume);
        AudioListener.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ChangeRegion(int i)
    {
        PhotonNetwork.ConnectToRegion(regions[i]);
        Debug.Log(PhotonNetwork.CloudRegion);
    }
}
