using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource defaultAuido;
   
    [SerializeField] private AudioClip shotShound;
    [SerializeField] private AudioClip reloadSound;
   

    public void playShotSound()
    {
        effectAudioSource.PlayOneShot(shotShound);
    }
    public void playReloadSound()
    {
        effectAudioSource.PlayOneShot(reloadSound);
    }
    public void Play(string soundName)
    {
        switch (soundName.ToLower())
        {
            case "reload":
                playReloadSound();
                break;
            case "shot":
                playShotSound();
                break;
            default:
                Debug.LogWarning($"⚠️ Âm thanh '{soundName}' chưa được định nghĩa trong AudioManager.");
                break;
        }
    }


    public void DefaultAudioManager()
    {
        defaultAuido.Play();
    }

    public void Mute()
    {
        defaultAuido.Stop();
        effectAudioSource.Stop();
    }
}

