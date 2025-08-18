using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public GameObject pauseMenuPanel;

    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider effectSlider;

    [Header("Buttons")]
    public Button backButton;

    [Header("Audio Manager")]
    public AudioManager audioManager;

    // Lưu trữ volume settings
    private float currentMusicVolume = 1f;
    private float currentEffectVolume = 1f;

    private void Start()
    {
        // Load saved settings hoặc sử dụng giá trị mặc định
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        currentEffectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);

        // Setup sliders
        if (musicSlider != null)
        {
            musicSlider.value = currentMusicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (effectSlider != null)
        {
            effectSlider.value = currentEffectVolume;
            effectSlider.onValueChanged.AddListener(SetEffectVolume);
        }

        // Setup back button
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToPauseMenu);
        }

        // Ẩn settings panel ban đầu
        settingsPanel.SetActive(false);

        // Áp dụng volume ban đầu
        ApplyVolumeToAllAudioSources();
    }

    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;

        // Cập nhật AudioManager
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
        }

        // Cập nhật tất cả AudioSource có tag Music hoặc tên chứa "music"
        UpdateMusicAudioSources(volume);

        // Lưu setting
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        currentEffectVolume = volume;

        // Cập nhật AudioManager
        if (audioManager != null)
        {
            audioManager.SetEffectVolume(volume);
        }

        // CẬP NHẬT TẤT CẢ AUDIOSOURCE EFFECT (QUAN TRỌNG!)
        UpdateEffectAudioSources(volume);

        // Lưu setting
        PlayerPrefs.SetFloat("EffectVolume", volume);
    }

    private void UpdateMusicAudioSources(float volume)
    {
        // Tìm tất cả AudioSource có tag "Music" hoặc tên chứa "music"
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.gameObject.name.ToLower().Contains("music") ||
                audioSource.gameObject.name.ToLower().Contains("background") ||
                audioSource.gameObject.CompareTag("Music"))
            {
                audioSource.volume = volume;
            }
        }
    }

    private void UpdateEffectAudioSources(float volume)
    {
        // Tìm tất cả AudioSource KHÔNG phải music
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            // Nếu KHÔNG phải music thì coi như effect
            if (!audioSource.gameObject.name.ToLower().Contains("music") &&
                !audioSource.gameObject.name.ToLower().Contains("background") &&
                !audioSource.gameObject.CompareTag("Music"))
            {
                // Đặt volume cho AudioSource này
                audioSource.volume = volume;
            }
        }
    }

    private void ApplyVolumeToAllAudioSources()
    {
        UpdateMusicAudioSources(currentMusicVolume);
        UpdateEffectAudioSources(currentEffectVolume);
    }

    public void ShowSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        // Refresh tất cả AudioSource khi mở settings
        ApplyVolumeToAllAudioSources();
    }

    public void BackToPauseMenu()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);

        // Lưu settings khi thoát
        PlayerPrefs.Save();
    }

    // Method để các script khác có thể gọi để áp dụng volume ngay lập tức
    public static void ApplyCurrentEffectVolume(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            float effectVol = PlayerPrefs.GetFloat("EffectVolume", 1f);
            audioSource.volume = effectVol;
        }
    }
}