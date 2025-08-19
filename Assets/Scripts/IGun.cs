using TMPro;

public interface IGun
{
    void SetEquipped(bool equipped);
    void SetAmmoText(TextMeshProUGUI text);
    void SetAudioManager(AudioManager audioManager);
    void AddAmmo(float amount);

    // ✨ mới
    bool CanShoot(); // kiểm tra đủ mana + ammo không
    void SetManaSystem(ManaSystem manaSystem); // gắn mana system
}
