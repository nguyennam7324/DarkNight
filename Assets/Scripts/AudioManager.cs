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

