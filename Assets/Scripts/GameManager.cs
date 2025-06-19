using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
     void Start()
    {
        MainMenu();
    }
    public void MainMenu()
    {
        mainMenu.SetActive(true); 
        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
  
    }
}
