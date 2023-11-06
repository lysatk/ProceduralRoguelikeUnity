using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField] private AudioClip projectileCollisionClip;

    private void Awake()
    {
        // Ensure there is only one instance of this object in the game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Start the background music
        PlayMusic(musicClip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true; // Music typically loops
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
}
