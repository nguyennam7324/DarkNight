using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    public GameObject gameOver;
   

    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        MainMenu();
    }
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
        audioManager.Mute();
    }

    // THÊM METHOD NÀY
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
}
