using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource defaultAudio;

    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip reloadSound;

    // Thêm volume controls
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float effectVolume = 1f;

    private void Start()
    {
        // Load saved volumes
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);

        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        defaultAudio.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        effectVolume = volume;
        effectAudioSource.volume = volume;
        PlayerPrefs.SetFloat("EffectVolume", volume);
    }

    private void UpdateVolumes()
    {
        defaultAudio.volume = musicVolume;
        effectAudioSource.volume = effectVolume;
    }

    public void playShotSound()
    {
        effectAudioSource.PlayOneShot(shotSound);
    }

    public void playReloadSound()
    {
        effectAudioSource.PlayOneShot(reloadSound);
    }

    public void DefaultAudioManager()
    {
        defaultAudio.Play();
    }

    public void Mute()
    {
        defaultAudio.Stop();
        effectAudioSource.Stop();
    }
}