using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro elements

public class SettingsManager : MonoBehaviour
{
    // Reference to your UI sliders
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider masterVolumeSlider;

    // Reference to TextMeshPro GUI elements
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI effectsVolumeText;
    public TextMeshProUGUI masterVolumeText;

    private void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        // Load saved settings, apply them to sliders, and update text
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
        UpdateSliderText(musicVolumeSlider.value, musicVolumeText);
    }

    public void OnEffectsVolumeChanged()
    {
        SaveEffectsVolume(effectsVolumeSlider.value);
        UpdateSliderText(effectsVolumeSlider.value, effectsVolumeText);
    }

    public void OnAmbientVolumeChanged()
    {
        SaveMasterVolume(masterVolumeSlider.value);
        UpdateSliderText(masterVolumeSlider.value, masterVolumeText);
    }

    // Utility method to update the slider value text
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
        return PlayerPrefs.GetFloat("MusicVolume", 1.0f); // Default to max volume
    }

    // Save and load methods for effects volume
    private void SaveEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        PlayerPrefs.Save();
    }

    private float LoadEffectsVolume()
    {
        return PlayerPrefs.GetFloat("EffectsVolume", 1.0f); // Default to max volume
    }

    // Save and load methods for master volume
    private void SaveMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    private float LoadMasterVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", 1.0f); // Default to max volume
    }
}

