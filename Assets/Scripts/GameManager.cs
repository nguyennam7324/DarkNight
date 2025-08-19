using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject expUI; // 👈 exp UI panel
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

    // 👉 Method này chỉ dùng để ẩn menu nếu cần gọi ở nơi khác
    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void StartGame()
    {
        HideMainMenu();           // ẩn menu
        expUI.SetActive(true);    // bật exp UI
        Time.timeScale = 1f;      // cho game chạy lại
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
