using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip musicClipMenu;
    [SerializeField] private AudioClip musicClipHub;
    [SerializeField] private AudioClip musicClipLevel;
    [SerializeField] private AudioClip enemyDeathClip;
   
    [SerializeField] private AudioClip playerDeathClip;

    [SerializeField] private AudioClip projectileCollisionClip;

    [SerializeField] private AudioClip playerHitClip;

    [SerializeField] private AudioClip enemyAttackClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }

        PlayMusic(musicClipMenu);
    }

    private void LoadVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        ChangeVolume(musicVolume, sfxVolume);
    }

    public void SaveVolumeSettings(float musicVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
        ChangeVolume(musicVolume, sfxVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void ChangeVolume(float musicVolume, float sfxVolume)
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    public void PlayEnemyDeathSound()
    {
        PlaySFX(enemyDeathClip);
    }

    public void PlayPlayerDeathSound()
    {
        PlaySFX(playerDeathClip);
    }

    public void PlayPlayerHitSound()
    {
        PlaySFX(playerHitClip);
    }

    public void PlayEnemyAttackSound()
    {
        PlaySFX(enemyAttackClip);
    }
}


