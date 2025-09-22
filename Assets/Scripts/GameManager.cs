using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
//using TMPro.EditorUtilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
   
    public GameObject gameOver;
    public static bool isPause;

    public static Vector3 bossPosition = new Vector3(50f, 0f, 0f);
    public static GameObject boss;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SettingsManager settingsManager; // Thêm reference
    internal static bool IsSpawnedCheckpoint;
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        if (settingsManager == null)
        {
            settingsManager = FindObjectOfType<SettingsManager>();
        }
    }

    void Start()
    {
        MainMenu();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false); // Ẩn settings
        GameManager.IsSpawnedCheckpoint = false;
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
        if (IsSpawnedCheckpoint && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
           ShowNPCSkillPoint();
           IsSpawnedCheckpoint = false;
        }
    }

    private void ShowNPCSkillPoint()
    {
        NPCSkill.instance.Show();
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);

        Time.timeScale = 0f;
        audioManager.Mute();
      
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
       
    }


    
    public void GameOver()
    {
        gameOver.SetActive(true);
        
       
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
       
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        audioManager.DefaultAudioManager();
       
    }


    public void OpenSettings()
    {
        if (settingsManager != null)
        {
            settingsManager.ShowSettings();
        }
    }

   
}

