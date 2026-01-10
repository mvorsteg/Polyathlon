using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsUI : BaseMenuUI
{
    [SerializeField]
    private Spinner unitsSpinner, qualitySpinner;
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
        
        unitsSpinner.FillWithEnum<SpeedUnits>();
        qualitySpinner.FillWithEnum<QualityLevel>();

        masterVolSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MASTER_VOL, 1f);
        sfxVolSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SOUNDS_VOL, 1f);
        musicVolSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MUSIC_VOL, 1f);

        mixer.SetFloat("masterVol", masterVolSlider.value);
        mixer.SetFloat("musicVol", musicVolSlider.value);
        mixer.SetFloat("soundsVol", sfxVolSlider.value);
    }

    protected override void Start()
    {
        base.Start();

        SpeedUnits units = (SpeedUnits)PlayerPrefs.GetInt(PlayerPrefsKeys.SPEED_UNITS, 0);
        if (EnumUtility.TryGetDescriptionFromValue(units, out string description))
        {
            unitsSpinner.SkipToValue(description);
        }

        QualityLevel quality = (QualityLevel)PlayerPrefs.GetInt(PlayerPrefsKeys.QUALITY_LEVEL, 0);
        if (EnumUtility.TryGetDescriptionFromValue(quality, out description))
        {
            QualitySettings.SetQualityLevel((int)quality);
            qualitySpinner.SkipToValue(description);
        }
    }

    public override void Navigate(MainMenuPlayer player, Vector2 input)
    {
        if (player.IsPrimary())
        {
            base.Navigate(player, input);

            if (input.x != 0)
            {
                if (EventSystem.current.currentSelectedGameObject == unitsSpinner.gameObject)
                {
                    unitsSpinner.Navigate(input.x > 0);
                }
                else if (EventSystem.current.currentSelectedGameObject == qualitySpinner.gameObject)
                {
                    qualitySpinner.Navigate(input.x > 0);
                }
            }
        }
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

    public void OnQualityChanged()
    {
        if (EnumUtility.TryGetValueFromDescription(qualitySpinner.Value, out QualityLevel level))
        {   
            PlayerPrefs.SetInt(PlayerPrefsKeys.QUALITY_LEVEL, (int)level);
            QualitySettings.SetQualityLevel((int)level);
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