using TMPro;

public interface IGun
{
    void SetEquipped(bool equipped);
    void SetAmmoText(TextMeshProUGUI text);
    void SetAudioManager(AudioManager audioManager);
    void AddAmmo(float amount);
}