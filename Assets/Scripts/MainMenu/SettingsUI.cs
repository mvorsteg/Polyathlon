using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : BaseMenuUI
{
    [SerializeField]
    private Spinner unitsSpinner;
    [SerializeField]
    private Slider masterVolSlider, sfxVolSlider, musicVolSlider;

    [SerializeField]
    private AudioMixer mixer;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        if (EnumUtility.TryGetDescriptionFromValue((SpeedUnits)PlayerPrefs.GetInt(PlayerPrefsKeys.SPEED_UNITS, 0), out string description))
        {
            unitsSpinner.SkipToValue(description);
        }
        masterVolSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MASTER_VOL, 1f);
        sfxVolSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SOUNDS_VOL, 1f);
        musicVolSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MUSIC_VOL, 1f);

        mixer.SetFloat("masterVol", masterVolSlider.value);
        mixer.SetFloat("musicVol", musicVolSlider.value);
        mixer.SetFloat("soundsVol", sfxVolSlider.value);
    }

    public void OnUnitsChanged()
    {
        if (EnumUtility.TryGetValueFromDescription(unitsSpinner.Value, out SpeedUnits units))
        {   
            PlayerPrefs.SetInt(PlayerPrefsKeys.SPEED_UNITS, (int)units);

            foreach (UI ui in FindObjectsByType<UI>(FindObjectsSortMode.None))
            {
                ui.SetSpeedUnit(units);
            }
        }
    }

    public void OnMasterVolChanged()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MASTER_VOL, masterVolSlider.value);
        mixer.SetFloat("masterVol", masterVolSlider.value);
    }

    public void OnSFXVolChanged()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SOUNDS_VOL, sfxVolSlider.value);
        mixer.SetFloat("soundsVol", sfxVolSlider.value);
    }

    public void OnMusicVolChanged()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MUSIC_VOL, musicVolSlider.value);
        mixer.SetFloat("musicVol", musicVolSlider.value);
    }
}