using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider masterVolumeSlider;

    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI effectsVolumeText;
    public TextMeshProUGUI masterVolumeText;

    private void Awake()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        musicVolumeSlider.value = LoadMusicVolume();
        UpdateSliderText(musicVolumeSlider.value, musicVolumeText);

        effectsVolumeSlider.value = LoadEffectsVolume();
        UpdateSliderText(effectsVolumeSlider.value, effectsVolumeText);

        masterVolumeSlider.value = LoadMasterVolume();
        UpdateSliderText(masterVolumeSlider.value, masterVolumeText);
    }

    public void OnMusicVolumeChanged()
    {
        SaveMusicVolume(musicVolumeSlider.value);
        UpdateAudioSystemVolume();
        UpdateSliderText(musicVolumeSlider.value, musicVolumeText);
    }

    public void OnEffectsVolumeChanged()
    {
        SaveEffectsVolume(effectsVolumeSlider.value);
        AudioSystem.Instance.PlayEnemyDeathSound();
        UpdateAudioSystemVolume();
        UpdateSliderText(effectsVolumeSlider.value, effectsVolumeText);
    }

    public void OnMasterVolumeChanged()
    {
        SaveMasterVolume(masterVolumeSlider.value);
        UpdateAudioSystemVolume();
        UpdateSliderText(masterVolumeSlider.value, masterVolumeText);
    }

    private void UpdateSliderText(float value, TextMeshProUGUI textElement)
    {
        int percentage = Mathf.RoundToInt(value * 100);
        textElement.text = percentage.ToString() + "%";
    }

    private void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    private float LoadMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    }

    private void SaveEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        PlayerPrefs.Save();
        
    }

    private float LoadEffectsVolume()
    {
        return PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
    }

    private void SaveMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    private float LoadMasterVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", 1.0f);
    }

    private void UpdateAudioSystemVolume()
    {
        if (AudioSystem.Instance != null)
        {
            AudioSystem.Instance.ChangeVolume(musicVolumeSlider.value, effectsVolumeSlider.value);
            AudioSystem.Instance.SetMasterVolume(masterVolumeSlider.value);
        }
    }

}
