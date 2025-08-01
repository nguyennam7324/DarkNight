using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    public GameObject gameOver;
    //[SerializeField] private AudioManger audioManager;

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
    public void StartGame()
    {
        mainMenu.SetActive(false);
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
