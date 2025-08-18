using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    //public void Play()
    //{

    //    //gameManager.StartGame();
    //    // Gọi LoadingScene để chuyển scene với loading
    //    LoadingScene.Instance.SwitchToScene(1); // hoặc scene id bạn muốn chuyển đến

    //}

    public void Play()
    {
        // Gọi trực tiếp loading với GameManager
        if (LoadingScene.Instance != null)
        {
            LoadingScene.Instance.StartGameWithLoading(gameManager);
        }
        else
        {
            Debug.LogError("LoadingScene.Instance is null!");
            gameManager.StartGame();
        }
    }
    public void Quit()
    {
        Debug.Log("Đang thoát game nè:");
        Application.Quit();
    }
    //public void MainMenu()
    //{
    //    SceneManager.LoadScene("SampleScene");
    //}
}
