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
    private float masterVolume = 1.0f;
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

        PlayMusicMenu();
    }

    private void LoadVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        ChangeVolume(musicVolume, sfxVolume);
        SetMasterVolume(masterVolume);

    }

    public void SaveVolumeSettings(float musicVolume, float sfxVolume, float masterVol)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVol);
        PlayerPrefs.Save();
        masterVolume = masterVol;
        ChangeVolume(musicVolume, sfxVolume);
    }

    public void ChangeVolume(float musicVolume, float sfxVolume)
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }


    public void PlayMusicMenu()
    {
        PlayMusic(musicClipMenu);
    }

    public void PlayMusicHub()
    {
        PlayMusic(musicClipHub);
    }

    public void PlayMusicLevel()
    {
        PlayMusic(musicClipLevel);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip)
        {
            return;
        }
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
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


    public void SetMasterVolume(float masterVol)
    {
        masterVolume = masterVol;
        ChangeVolume(musicSource.volume, sfxSource.volume);
    }

}


