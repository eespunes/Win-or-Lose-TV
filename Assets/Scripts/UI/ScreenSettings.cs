using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    private Resolution[] _resolutions;

    [SerializeField] private TMP_Dropdown resolutionDropdown;


    private void Start()
    {
        GenerateResolutionsDropdown();
    }

    private void GenerateResolutionsDropdown()
    {
        _resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        int currentResolution = 0;
        int i = 0;

        foreach (var resolution in _resolutions)
        {
            options.Add(resolution.width + "x" + resolution.height + " @" + resolution.refreshRate);

            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height &&
                resolution.refreshRate == Screen.currentResolution.refreshRate)
                currentResolution = i;
            i++;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void ChangeScreen()
    {
        StartCoroutine("TargetDisplay");
    }
    
    private IEnumerator TargetDisplay()
    {
        if (Display.displays.Length > 1)
        {
            // Get the current screen resolution.
            int currentScreen = PlayerPrefs.GetInt("UnitySelectMonitor");
            int nextScreen = currentScreen == 0 ? 1 : 0;
            int screenWidth = Display.displays[nextScreen].systemWidth;
            int screenHeight = Display.displays[nextScreen].systemHeight;
    
            // Set the target display and a low resolution.
            PlayerPrefs.SetInt("UnitySelectMonitor", nextScreen);
            Screen.SetResolution(800, 450, Screen.fullScreen);
    
            // Wait a frame.
            yield return null;
            // Restore resolution.
            Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreen);
            
            currentScreen = PlayerPrefs.GetInt("UnitySelectMonitor");

            string[] endings = new string[]{
                "exe", "x86", "x86_64", "app"
            };
            string executablePath = Application.dataPath + "/..";
            foreach (string file in System.IO.Directory.GetFiles(executablePath)) {
                foreach (string ending in endings) {
                    if (file.ToLower ().EndsWith ("." + ending)) {
                        Process.Start (executablePath + file);
                        Application.Quit();
                    }
                }
             
            }
        }
    }
}