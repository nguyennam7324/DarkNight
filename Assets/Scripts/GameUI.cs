using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public void Play()
    {
        gameManager.StartGame();
    }
    public void Quit()
    {
        Application.Quit();
    }
    //public void MainMenu()
    //{
    //    SceneManager.LoadScene("SampleScene");
    //}
}
