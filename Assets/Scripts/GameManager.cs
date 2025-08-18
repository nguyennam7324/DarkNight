using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject expUI; // 👈 thêm exp UI
    public GameObject gameOver;

    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        MainMenu();
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        expUI.SetActive(false);   // 👈 ẩn exp UI khi vào menu
        Time.timeScale = 0f;
        audioManager.Mute();
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        expUI.SetActive(true);   // 👈 hiện exp UI khi bắt đầu game
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
