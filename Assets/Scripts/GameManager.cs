// using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    [SerializeField] private GameObject gameOver;
//    private bool isGameOver = false;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        gameOver.SetActive(false);
//    }

//    public void GameOver()
//    {
//        isGameOver = true;
//        Time.timeScale = 0;
//        gameOver.SetActive(true);
//    }
//    public void RestarGame()
//    {
//        isGameOver = false;
//        Time.timeScale = 1;
//        SceneManager.LoadScene("Scene1");
//    }
//    public void GotoMenu()
//    {
//        Time.timeScale = 1;
//        SceneManager.LoadScene("Menu");
//    }
//}
