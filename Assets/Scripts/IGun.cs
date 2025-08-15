using TMPro;

public interface IGun
{
    void AddAmmo(float amount);
    void SetEquipped(bool equipped);
    void SetAmmoText(TextMeshProUGUI text);
    void SetAudioManager(AudioManager manager);

    // Thêm dòng này để GunHolder gọi được
    void SetOriginalLocalPos();
}
