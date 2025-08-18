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

    private void Start()
    {
        // Setup sliders
        if (musicSlider != null)
        {
            musicSlider.value = audioManager.musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (effectSlider != null)
        {
            effectSlider.value = audioManager.effectVolume;
            effectSlider.onValueChanged.AddListener(SetEffectVolume);
        }

        // Setup back button
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToPauseMenu);
        }

        // Ẩn settings panel ban đầu
        settingsPanel.SetActive(false);
    }

    public void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
    }

    public void SetEffectVolume(float volume)
    {
        audioManager.SetEffectVolume(volume);
    }

    public void ShowSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
}