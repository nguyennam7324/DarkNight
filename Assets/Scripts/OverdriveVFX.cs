using UnityEngine;

public class OverdriveVFX : MonoBehaviour
{
    public ParticleSystem overdriveParticles;
    public GameObject screenGlow; // a UI image or sprite for glowing border
    public AudioSource overdriveSound;

    public void Play()
    {
        if (overdriveParticles != null) overdriveParticles.Play();
        if (screenGlow != null) screenGlow.SetActive(true);
        if (overdriveSound != null) overdriveSound.Play();
        Debug.Log("OverdriveVFX Play() called");
        // optionally start chromatic aberration, camera shake, etc.
    }

    public void Stop()
    {
        if (overdriveParticles != null) overdriveParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (screenGlow != null) screenGlow.SetActive(false);
        if (overdriveSound != null) overdriveSound.Stop();
    }
}
