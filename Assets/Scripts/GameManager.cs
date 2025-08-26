using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject goldUI; // Thêm tham chiếu đến Gold UI
    public GameObject gameOver;
    public static bool isPause;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SettingsManager settingsManager; // Thêm reference


    void Start()
    {
        MainMenu();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false); // Ẩn settings
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
 
            {
                PauseGame();
            }
        }
    }
    public void MainMenu()
    {
        mainMenu.SetActive(true);

        Time.timeScale = 0f;
        audioManager.Mute();
        // Ẩn Gold UI khi ở main menu
        if (goldUI != null)
        {
            goldUI.SetActive(false);
        }
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }
    public void StartGame()
    {
        // Không cần mainMenu.SetActive(false) nữa vì đã ẩn rồi

        
        Time.timeScale = 1f;      
        audioManager.DefaultAudioManager();
        // Hiện Gold UI khi bắt đầu game
        if (goldUI != null)
        {
            goldUI.SetActive(true);
        }
    }


    
    public void GameOver()
    {
        gameOver.SetActive(true);
        // Có thể ẩn Gold UI khi game over nếu muốn
        if (goldUI != null)
        {
            goldUI.SetActive(false);
        }
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
        audioManager.Mute();
        if (goldUI != null)
        {
            goldUI.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        audioManager.DefaultAudioManager();
        // Hiện lại Gold UI khi resume
        if (goldUI != null)
        {
            goldUI.SetActive(true);
        }
    }


    public void OpenSettings()
    {
        if (settingsManager != null)
        {
            settingsManager.ShowSettings();
        }
    }
}

